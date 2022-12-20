# CosmosSample

Sample application to interact with cosmosdb by and using azure key vault to strore secrets.

You need to provision:
1. CosmosDb
2. Azure key vault
  - cosmodb-url : url secret
  - cosmosdb-key : Key secret
3. Register your application 
4. Provide access of the application created in step 3 in azure key vault
