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
	public class ProductTypeController : BaseController
	{
		private readonly IProductTypeService _productTypeService;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public ProductTypeController(IMapper mapper, ILogger<ProductTypeController> logger, IProductTypeService productTypeService)
		{
			_mapper = mapper;
			_logger = logger;
			_productTypeService = productTypeService;
		}

		[HttpPut]
		[Route("{productTypeId:int}")]
		public async Task<IActionResult> UploadSurchargeRate(int productTypeId, ProductTypeDto productTypeUpdate)
		{
			if (productTypeId != productTypeUpdate.ProductTypeId) return BadRequest();

			if (!ModelState.IsValid) return BadRequest(ModelState);

			var entity = await _productTypeService.GetById(productTypeId);
			if (entity == null) return NotFound();

			await _productTypeService.Update(_mapper.Map<ProductTypeDto, ProductType>(productTypeUpdate, entity));

			_logger.LogInformation("Upload SurchargeRate for product type");
			return NoContent();
		}
	}
}