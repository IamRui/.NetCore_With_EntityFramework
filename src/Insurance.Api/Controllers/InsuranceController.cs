using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Insurance.Business.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Insurance.Api.Models;
using Insurance.Data.Access.Entities;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Controllers
{
	[Route("api/[controller]")]
	public class InsuranceController : BaseController
	{
		private readonly IProductService _productService;
		private readonly ICalculationService _calculationService;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public InsuranceController(IMapper mapper, ILogger<InsuranceController> logger, IProductService productService,
			ICalculationService calculationService)
		{
			_mapper = mapper;
			_logger = logger;
			_productService = productService;
			_calculationService = calculationService;
		}

		[HttpPost]
		[Route("product/{productId:int}")]
		public async Task<IActionResult> CalculateProductInsurance(int productId)
		{
			var productDetails = await _productService.GetById(productId);
			if (productDetails == null) return NotFound();

			var insuranceCost = _calculationService.GetInsuranceForProduct(productDetails);
			if (insuranceCost == null) return BadRequest();

			var productInsurance = _mapper.Map<ProductInsuranceDto>(productDetails);
			productInsurance.InsuranceValue = insuranceCost;

			_logger.LogInformation("Get insurance cost for product");
			return Ok(productInsurance);
		}

		[HttpPost]
		[Route("order/products")]
		public async Task<IActionResult> CalculateOrderInsurance(IEnumerable<int> productIdList)
		{
			var productEntityList = new List<Product>();
			foreach (var productId in productIdList)
			{
				var entity = await _productService.GetById(productId);
				if (entity == null) return NotFound();

				productEntityList.Add(entity);
			}
			var orderInsuranceCost = _calculationService.GetInsuranceForOrder(productEntityList.AsEnumerable());
			if (orderInsuranceCost == null) return BadRequest();

			_logger.LogInformation("Get insurance cost for order");
			return Ok(orderInsuranceCost);
		}
	}
}