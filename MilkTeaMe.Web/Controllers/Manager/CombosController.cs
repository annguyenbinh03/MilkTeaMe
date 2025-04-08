using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaMe.Repositories.DbContexts;
using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Services.Implementations;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Web.Models.Requests;
using MilkTeaMe.Web.Models.Responses;
using Newtonsoft.Json;

namespace MilkTeaMe.Web.Controllers.Manager
{
    public class CombosController : Controller
    {
        private readonly IProductService _productService;
        private readonly CloudinaryService _cloudinaryService;

        public CombosController(IProductService productService, CloudinaryService cloudinaryService)
        {
            _productService = productService;
            _cloudinaryService = cloudinaryService;
        }

        // GET: Combos
        public async Task<IActionResult> Index(int? page,
            string? search)
        {
            int pageSize = 3;
            int pageNumber = page ?? 1;

            var (products, totalItems) = await _productService.GetCombos(search, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.Search = search;
            return View(products);
        }

        // GET: Combos/Create
        public async Task<IActionResult> Create()
        {

            var (milkteas, totalItems1) = await _productService.GetMilkTeas(null);
            var (toppings, totalItems2) = await _productService.GetToppings(null);

            var milkTeaViewModels = milkteas.Select(mt => new ComboResponse
            {
                Id = mt.Id,
                Name = mt.Name,
                Price = null,
                ImageUrl = mt.ImageUrl,
                IsMilkTea = true,
                Sizes = mt.ProductSizes.Select(ps => new ProductSizeViewModel
                {
                    SizeId = ps.SizeId,
                    SizeName = ps.Size.Name,
                    Price = ps.Price
                }).ToList()
            }).ToList();

            var toppingViewModels = toppings.Select(t => new ComboResponse
            {
                Id = t.Id,
                Name = t.Name,
                Price = t.Price,
                ImageUrl = t.ImageUrl,
                IsMilkTea = false,
                Sizes = new List<ProductSizeViewModel>()
            }).ToList();

            List<ComboResponse> productViewModels = milkTeaViewModels.Concat(toppingViewModels).ToList();
            ViewBag.Products = productViewModels;
            ViewBag.Milkteas = productViewModels.Where(p => p.IsMilkTea).ToList();
            ViewBag.Toppings = productViewModels?.Where(p => !p.IsMilkTea).ToList();

            return View();
        }

        // POST: Combos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Image")] ComboCreationRequest request, [FromForm] string Products)
        {
            if (!string.IsNullOrEmpty(Products))
            {
                request.Products = JsonConvert.DeserializeObject<List<ProductInComboCreationRequest>>(Products) ?? new List<ProductInComboCreationRequest>();
            }
            else
            {
                request.Products = JsonConvert.DeserializeObject<List<ProductInComboCreationRequest>>(Products) ?? new List<ProductInComboCreationRequest>();

                var (milkteas, totalItems1) = await _productService.GetMilkTeas(null);
                var (toppings, totalItems2) = await _productService.GetToppings(null);

                var milkTeaViewModels = milkteas.Select(mt => new ComboResponse
                {
                    Id = mt.Id,
                    Name = mt.Name,
                    Price = null,
                    ImageUrl = mt.ImageUrl,
                    IsMilkTea = true,
                    Sizes = mt.ProductSizes.Select(ps => new ProductSizeViewModel
                    {
                        SizeId = ps.SizeId,
                        SizeName = ps.Size.Name,
                        Price = ps.Price
                    }).ToList()
                }).ToList();

                var toppingViewModels = toppings.Select(t => new ComboResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    Price = t.Price,
                    ImageUrl = t.ImageUrl,
                    IsMilkTea = false,
                    Sizes = new List<ProductSizeViewModel>()
                }).ToList();

                List<ComboResponse> productViewModels = milkTeaViewModels.Concat(toppingViewModels).ToList();
                ViewBag.Products = productViewModels;
                ViewBag.Milkteas = productViewModels.Where(p => p.IsMilkTea).ToList();
                ViewBag.Toppings = productViewModels?.Where(p => !p.IsMilkTea).ToList();
                return View(request);
            }

            string? imageUrl = null;
            if (request.Image != null && request.Image.Length > 0)
            {
                using var stream = request.Image.OpenReadStream();
                imageUrl = await _cloudinaryService.UploadImageAsync(stream, request.Name);
                if (imageUrl == null)
                {
                    ModelState.AddModelError("Image", "Không thể tải ảnh lên.");
                    return View(request);
                }
            }

            var model = new ComboModel
            {

                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = imageUrl ?? "",
                Products = new List<ProductInCombo>()
            };

            if (request.Products != null)
            {
                foreach (var productRequest in request.Products)
                {
                    var product = new ProductInCombo
                    {
                        Id = productRequest.ProductId,
                        Quantity = productRequest.Quantity,
                        Size = productRequest.SizeName
                    };
                    model.Products.Add(product);
                }
            }

            try
            {
                await _productService.CreateCombo(model);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: MilkTeas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            await _productService.Delete((int)id);
            return RedirectToAction(nameof(Index));
        }

        // GET: MilkTeas/Active/5	
        public async Task<IActionResult> Active(int? id)
        {
            if (id == null)
                return NotFound();
            await _productService.Active((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}
