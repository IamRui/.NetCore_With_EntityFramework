# To run the project
1. Open command prompt, run command cd 'project path'
2. Run command 'dotnet run'
3. It will display the port on which the API is running.
4. Open a browser and navigate to  http://localhost:5001/swagger/index.html (here port no. is 5001)
5. The Swagger UI is displayed where the API endpoints can be consumed.

# API endpoints
1. Get product insurance cost e.g. Post: https://localhost:5001/api/Insurance/product/{productId}. ProductId is required and response body inclues productId, insuranceValue, productTypeName, productTypeHasInsurance, salesPrice.
2. Get order insurance cost e.g. Post: https://localhost:5001/api/Insurance/order/products. A list of product ids should be included in the request body and response body includes the value of the order insurance cost.
3. Upload surcharge rate e.g. Put: https://localhost:5001/api/ProductType/{productTypeId}. ProductTypeId is required and the request body should have productTypeId and surcharge rate. This endpoint returns 202 NoContent as response.

# Nuget Packages Used
1. Mapping: Automapper 10.1.1
2. API documentation: Swashbuckle.AspNetCore 6.1.4
3. Unit tests/Mocking library: Xunit 2.4.1 & NSubstitute 4.2.2 & Microsoft.Data.Sqlite 5.0.6
4. Data access: Microsoft.EntityFrameworkCore 5.0.6 & Microsoft.EntityFrameworkCore.SqlServer 5.0.6
