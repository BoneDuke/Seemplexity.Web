using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Seemplexity.Avalon.BusinesLogic.Services;
using Seemplexity.BusinesLogic.Model;
using Seemplexity.BusinesLogic.Services;
using Seemplexity.Common.Excel;
using Seemplexity.Common.Services;
using Seemplexity.Web.Models.Admin;
using Seemplexity.Web.Utils;

namespace Seemplexity.Web.Controllers
{
    [Authorize(Roles = "Admin,AdminPartner")]
    public class AdminController : Controller
    {
        private readonly AspUsersService _usersService;
        private readonly TransportService _transportService;
        private readonly TouristsService _touristsService;
        private readonly HotelMappingService _hotelMappingService;
        private readonly AvalonHotelMappingService _avalonHotelMappingService;
        private readonly ExcelModelService _excelModelService;
        private readonly ReservationCreationService _reservationCreationService;

        private readonly ExcursionMappingService _excursionMappingService;
        private readonly AvalonExcursionMappingService _avalonExcursionMappingService;

        public AdminController()
        {
            _usersService = new AspUsersService();
            _transportService = new TransportService();
            _touristsService = new TouristsService();
            _hotelMappingService = new HotelMappingService();
            _excelModelService = new ExcelModelService();
            _avalonHotelMappingService = new AvalonHotelMappingService();
            _reservationCreationService = new ReservationCreationService();

            _excursionMappingService = new ExcursionMappingService();
            _avalonExcursionMappingService = new AvalonExcursionMappingService();
        }

        public ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("ViewUsers");
        }

        #region BusDescriptions

        [HttpPost]
        public ActionResult EditBusDescription(BusDescription model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new SeemplexityModel())
                {
                    context.BusDescriptions.AddOrUpdate(model);
                    context.SaveChanges();
                }
            }
            return null;
        }

        [HttpGet]
        public ActionResult EditBusDescription(int id)
        {
            using (var context = new SeemplexityModel())
            {
                var desc = context.BusDescriptions.AsQueryable().Include(d => d.SchemeFloorDescriptions).SingleOrDefault(d => d.Id == id);
                if (desc != null)
                    return View(desc);
            }
            return null;
        }

        public ActionResult DetailsBusDescription(int id)
        {
            using (var context = new SeemplexityModel())
            {
                var desc = context.BusDescriptions.SingleOrDefault(d => d.Id == id);
                if (desc != null)
                    return View(desc);
            }
            return null;
        }

        public ActionResult DeleteBusDescription(int id)
        {
            using (var context = new SeemplexityModel())
            {
                var desc = context.BusDescriptions.SingleOrDefault(d => d.Id == id);
                if (desc != null)
                    context.BusDescriptions.Remove(desc);
                context.SaveChanges();
            }
            return RedirectToAction("ViewBusDescriptions");
        }

        [HttpPost]
        public ActionResult CreateBusDescription(BusDescription model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new SeemplexityModel())
                {
                    context.BusDescriptions.Add(model);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("ViewBusDescriptions");
        }

        [HttpGet]
        public ActionResult CreateBusDescription()
        {
            return View();
        }

        public ActionResult ViewBusDescriptions()
        {
            using (var context = new SeemplexityModel())
            {
                var model = context.BusDescriptions.ToArray();
                return View(model);
            }
        }

        #endregion



        public ActionResult ViewBusTours()
        {
            List<int> tourTypeKeys = null;
            var model = new BusTourViewModel();

            var settingValue = SettingsManager.ApplicationSettings[SettingsManager.BusTourType];
            if (!string.IsNullOrEmpty(settingValue))
                tourTypeKeys = settingValue.Split(',').Select(int.Parse).ToList();

            if (tourTypeKeys != null)
            {
                using (var context = new Avalon.BusinesLogic.Avalon())
                {
                    var keys = tourTypeKeys;
                    model.TourTypes.AddRange(context.TipTurs.Where(t => keys.Contains(t.TP_KEY))
                            .Select(t => new KeyValueViewModel()
                            {
                                Id = t.TP_KEY,
                                Name = t.TP_NAME
                            })
                            .ToList()
                            .OrderByDescending(t => t.Id));

                    model.Tours.AddRange(context.tbl_TurList.Where(t => keys.Contains(t.TL_TIP))
                            .Select(t => new KeyValueViewModel()
                            {
                                Id = t.TL_KEY,
                                Name = t.TL_NAME
                            })
                            .ToList()
                            .OrderByDescending(t => t.Id));
                }
            }

            return View(model);
        }

        public ActionResult ViewUsers()
        {
            using (var context = new SeemplexityModel())
            {
                var users = context.AspNetUsers.Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    EMail = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    RoleNames = u.AspNetRoles.Select(r => r.Name).ToList(),
                    UserName = u.UserName,
                    EMailConfirmed = u.EmailConfirmed
                }).ToList();

                return View(users);
            }
        }

        [HttpPost]
        public ActionResult EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(model.User.Id);

                if (user == null)
                    throw new ArgumentException($"Пользователь с таким Id не найден {model.User.Id}");

                UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());

                if (model.SelectedRoleNames != null)
                {
                    var result = UserManager.AddToRoles(user.Id, model.SelectedRoleNames);
                    if (!result.Succeeded)
                        throw new ArgumentException($"Пользователя с Id {model.User.Id} не удалось добавить в роли");
                }
                

                user.UserName = model.User.UserName;
                user.PhoneNumber = model.User.PhoneNumber;
                user.Email = model.User.EMail;
                user.EmailConfirmed = model.User.EMailConfirmed;
                UserManager.Update(user);

                return RedirectToAction("ViewUsers");
            }
            return null;
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
            using (var context = new SeemplexityModel())
            {
                var user = context.AspNetUsers.SingleOrDefault(u => u.Id == id);

                if (user != null)
                {
                    var model = new EditUserViewModel
                    {
                        User = new UserViewModel()
                        {
                            Id = user.Id,
                            EMail = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            RoleNames = user.AspNetRoles.Select(r => r.Name).ToList(),
                            UserName = user.UserName,
                            EMailConfirmed = user.EmailConfirmed
                        },
                        Roles = context.AspNetRoles.ToList().Select(r => r.Name).ToList()
                    };

                    return View(model);
                }
                    
            }
            return null;
        }

        [HttpGet]
        public ActionResult CreateBusScheme(int busDescriptionId)
        {
            List<BusSchemeFloorDescription> model = null;
            using (var context = new SeemplexityModel())
            {
                var descriptions = context.BusSchemeFloorDescriptions
                    .Include(d => d.BusDescription)
                    .Include(d => d.Items)
                    .Where(d => d.BusDescription.Id == busDescriptionId).ToList();
                if (descriptions.Any())
                    model = descriptions;
            }
            if (model == null)
            {
                model = new List<BusSchemeFloorDescription>();
                var floor = new BusSchemeFloorDescription
                {
                    ColumnsCount = 5,
                    RowsCount = 10,
                    FloorNumber = 1,
                    Items = new List<BusSchemeItem>(),
                    BusDescription = new BusDescription()
                    {
                        Id = busDescriptionId
                    }
                };

                int position = 1;
                for (var row = 0; row < floor.RowsCount; row++)
                {
                    for (var col = 0; col < floor.ColumnsCount; col++)
                    {
                        floor.Items.Add(new BusSchemeItem()
                        {
                            ColNumber = col,
                            RowNumber = row,
                            IsUsable = true,
                            Value = (position++).ToString()
                        });
                    }
                }

                model.Add(floor);
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateBusScheme(int busDescriptionId, ICollection<BusSchemeFloorDescription> floorDescriptions)
        {
            using (var context = new SeemplexityModel())
            {
                var description = context.BusDescriptions.SingleOrDefault(d => d.Id == busDescriptionId);
                if (description != null)
                {
                    context.BusSchemeFloorDescriptions.RemoveRange(description.SchemeFloorDescriptions);
                    foreach (var schemeDesc in floorDescriptions)
                    {
                        description.SchemeFloorDescriptions.Add(schemeDesc);
                    }

                    context.SaveChanges();
                }
            }

            return RedirectToAction("ViewBusDescriptions");
        }

        [HttpPost]
        public ActionResult CreateBusSchemeFloor(BusSchemeFloorDescription desc)
        {
            var test = false;
            return null;
        }

        [HttpGet]
        public ActionResult CreateBusSchemeFloor(int busDescriptionId, int floorNumber, int? rowsCount, int? columnsCount)
        {
            var floor = new BusSchemeFloorDescription
            {
                ColumnsCount = columnsCount ?? 5,
                RowsCount = rowsCount ?? 10,
                FloorNumber = floorNumber,
                Items = new List<BusSchemeItem>(),
                BusDescription = new BusDescription()
                {
                    Id = busDescriptionId
                }
            };

            int position = 1;
            for (var row = 0; row < floor.RowsCount; row++)
            {
                for (var col = 0; col < floor.ColumnsCount; col++)
                {
                    floor.Items.Add(new BusSchemeItem()
                    {
                        ColNumber = col,
                        RowNumber = row,
                        IsUsable = true,
                        Value = (position++).ToString()
                    });
                }
            }

            return PartialView(floor);
        }

        [HttpGet]
        public ActionResult UploadExcursionFile(string dgCodes)
        {
            ViewBag.DgCodes = dgCodes;
            return View();
        }

        private ActionResult ProcessExcursionFile(string path)
        {
            ExcelExcursionModel model = _excelModelService.GetExcursionModelFromFile(path);
            _excursionMappingService.MapTouristRows(model);
            _avalonExcursionMappingService.MapTouristRows(model);

            _hotelMappingService.MapTouristRows(model);
            _avalonHotelMappingService.MapTouristRows(model);

            if (model.Tourists.Any(t => t.AvalonHotelKey == null || t.AvalonExcursionKey == null))
            {
                var compareHotelsViewModel = new CompareExcursionsViewModel
                {
                    CompareExcursionsModel = model.Tourists.Select(t => new ExcursionMapping
                    {
                        ExcursionName = t.ExcursionName,
                        AvalonExcursionKey = t.AvalonExcursionKey ?? -1
                    }).Distinct(new ExcursionMappingComparer()).ToList(),
                    CompareHotelsModel = model.Tourists.Select(t => new HotelMapping
                    {
                        HotelName = t.HotelName,
                        AvalonHotelKey = t.AvalonHotelKey ?? -1,
                        ResortName = string.Empty,
                        PartnerType = PartnerType.Excursion
                    }).Distinct(new HotelMappingComparer()).ToList(),
                    Hotels = _avalonHotelMappingService.GetHotelsKeyName(4),
                    Excursions = _avalonExcursionMappingService.GetExcursionsKeyName(4),
                    ExcelFilePath = path
                };

                return View("CompareExcursions", compareHotelsViewModel);
            }

            var uploadModel = new UploadExcursionsListViewModel
            {
                ExcelModel = model,
                Hotels = _avalonHotelMappingService.GetHotelsKeyName(4),
                Excursions = _avalonExcursionMappingService.GetExcursionsKeyName(4)
            };

            return View("ConfirmUploadExcursionsList", uploadModel);
        }

        [HttpPost]
        public ActionResult UploadExcursionFile()
        {

            if (Request.Files.Count > 0)
            {
                var path = SaveFile(Request.Files[0], "excursion");
                return ProcessExcursionFile(path);
            }

            return new EmptyResult();
            
        }

        [HttpPost]
        public ActionResult CompareExcursions(CompareExcursionsResultModel model)
        {
            _excursionMappingService.UpdateHotelMappings(model.CompareExcursions);
            foreach (var m in model.CompareHotels)
            {
                m.PartnerType = PartnerType.Excursion;
                m.ResortName = string.Empty;
            }
            _hotelMappingService.UpdateHotelMappings(model.CompareHotels);

            return ProcessExcursionFile(model.ExcelFilePath);
        }

        [HttpGet]
        public ActionResult UploadTransferFile(string dgCodes)
        {
            ViewBag.DgCodes = dgCodes;
            return View();
        }

        private string SaveFile(HttpPostedFileBase file, string prefix = "")
        {
            var path = string.Empty;
            if (file != null && file.ContentLength > 0 && file.FileName.Contains(".xls"))
            {
                var fileName = Path.GetFileName(file.FileName);
                if (!string.IsNullOrEmpty(fileName))
                {
                    var filePath = Server.MapPath($"~/UploadedFiles/{DateTime.Now.Date.ToString("dd.MM.yyyy")}");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    path = Path.Combine(filePath, $"[{User.Identity.Name}]_{prefix}_{fileName}");
                    file.SaveAs(path);
                }
            }

            return path;
        }

        private ActionResult ProcessTansferFile(string path)
        {
            var model = _excelModelService.GetTransferModelFromFile(path);
            _hotelMappingService.MapTouristRows(model);
            _avalonHotelMappingService.MapTouristRows(model);

            if (model.Tourists.SelectMany(t => t).Any(t => t.AvalonHotelKey == null))
            {
                var compareHotelsModel = new List<List<HotelMapping>>();
                foreach (var tourists in model.Tourists)
                {
                    var ts = tourists
                        .Select(t => new HotelMapping()
                        {
                            ResortName = t.Resort,
                            HotelName = t.HotelName,
                            PartnerType = model.Type,
                            AvalonHotelKey = t.AvalonHotelKey ?? -1
                        })
                        .Distinct(new HotelMappingComparer())
                        .ToList();

                    compareHotelsModel.Add(ts);
                }

                var compareHotelsViewModel = new CompareHotelsViewModel
                {
                    CompareHotelsModel = compareHotelsModel,
                    Hotels = _avalonHotelMappingService.GetHotelsKeyName(4),
                    ExcelFilePath = path
                };

                return View("CompareHotels", compareHotelsViewModel);
            }

            var uploadModel = new UploadTouristsListViewModel()
            {
                ExcelModel = model,
                Hotels = _avalonHotelMappingService.GetHotelsKeyName(4)
            };

            return View("ConfirmUploadTouristsList", uploadModel);
        }

        [HttpPost]
        public ActionResult UploadTransferFile()
        {
            
            if (Request.Files.Count > 0)
            {
                var path = SaveFile(Request.Files[0]);
                return ProcessTansferFile(path);
            }

            return new EmptyResult();
 
        }

        [HttpPost]
        public ActionResult CompareHotels(CompareHotelsResultModel model)
        {
            _hotelMappingService.UpdateHotelMappings(model.CompareHotels);
            return ProcessTansferFile(model.ExcelFilePath);
        }

        private void PreUpdateModel(ExcelTransferModel excelModel)
        {
            foreach (var tourists in excelModel.Tourists)
            {
                foreach (var t in tourists)
                {
                    if (t.Name == null)
                        t.Name = string.Empty;
                    if (t.Surname == null)
                        t.Surname = string.Empty;
                }
            }
        }

        [HttpPost]
        public ActionResult UploadTouristsList(ExcelTransferModel excelModel)
        {
            PreUpdateModel(excelModel);
            var dgCodes = _reservationCreationService.SaveExcelModel(excelModel);
            return RedirectToAction("UploadTransferFile", new { dgCodes = string.Join(",", dgCodes) });
        }

        [HttpPost]
        public ActionResult UploadExcursionsList(ExcelExcursionModel excelModel)
        {
            var dgCodes = _reservationCreationService.SaveExcelModel(excelModel);
            return RedirectToAction("UploadExcursionFile", new { dgCodes = string.Join(",", dgCodes) });
        }
    }
}