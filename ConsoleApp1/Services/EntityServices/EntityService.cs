
using Domain.Aggregates.Lines;
using Domain.Aggregates.Products;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using SharedLibrary.OperationResult;

namespace ConsoleApp1.Services.EntityServices
{
    internal class EntityService<TService>
    {
        private static async Task<TId> SaveToDatabase<TEntity, TId>(
            ILogger<TService> logger,
            IGenericEntityDAL<TEntity, TId> entityDAL,
            TEntity entity)
            where TEntity : class
            where TId : struct
        {
            OperationResult<TId> idResult = await entityDAL.CreateAsync(entity);
            if (idResult.IsSuccess == false)
                logger.LogError(idResult.Error.ErrorCode);
            return idResult.Value;
        }

        internal static async Task<ProductionLineId> CreateProductionLine(string name, ILogger<TService> logger, IProductionLineDAL lineDAL)
        {
            var line = ProductionLine.Create(isActive: true, name);
            var id = await SaveToDatabase(logger, lineDAL, line);
            return new ProductionLineId(id);
        }

        internal static async Task<ProductionLineEntity> GetProductionLineById(ILogger<TService> logger, ProductionLineId id, IProductionLineDAL lineDAL)
        {
            var lineResult = await lineDAL.GetByAsync(x => x.Id == id, isTracking: true);
            if (lineResult.IsSuccess)
                logger.LogError(lineResult.Error.ErrorCode);
            return lineResult.Value;
        }

        internal static async Task<ProductId> CreateProduct(string name, GTIN gtin, int serialLength, ILogger<TService> logger, IProductDAL productDAL, ProductionLineEntity? line = null)
        {
            var product = Product.Create(name, gtin, serialLength);
            if (line is not null) 
                product.ProductionLines.Add(line);
            var id = await SaveToDatabase(logger, productDAL, product);
            return new ProductId(id);
        }
    }
}
