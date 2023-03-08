using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

var manager = new AmazonSecretsManagerClient();

var listVersionRequest = new ListSecretVersionIdsRequest()
{
    SecretId = "MySecretKey",
    IncludeDeprecated = true // to include old values
};

var listVersionResponse = await manager.ListSecretVersionIdsAsync(listVersionRequest);

var request = new GetSecretValueRequest()
{
    SecretId = "MySecretKey",
    // VersionStage = "AWSPREVIOUS" // you can either use "AWSCURRENT" or "AWSPREVIOUS" for current and previous versions, for older values use VersionId property
    VersionId = "bcb713d6-f88a-4730-b3fa-385345e43257"
};

var response = await manager.GetSecretValueAsync(request);

Console.WriteLine(response.SecretString);

var describeSecretRequest = new DescribeSecretRequest()
{
    SecretId = "MySecretKey"
};

var describeResponse = await manager.DescribeSecretAsync(describeSecretRequest); // you don't get the value but you get a lot of information about the secret

Console.WriteLine();