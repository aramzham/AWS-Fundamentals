using Amazon.S3;
using Amazon.S3.Model;

namespace Customers.S3.Api.Services;

public class CustomerImageService : ICustomerImageService
{
    private readonly IAmazonS3 _s3;
    private const string BucketName = "myawscoursebucket";

    public CustomerImageService(IAmazonS3 s3)
    {
        _s3 = s3;
    }

    public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file)
    {
        var putObjectRequest = new PutObjectRequest()
        {
            BucketName = BucketName,
            Key = $"images/{id}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream(),
            Metadata =
            {
                ["x-amz-meta-originalname"] = file.FileName,
                ["x-amz-meta-extension"] = Path.GetExtension(file.FileName)
            }
        };

        return await _s3.PutObjectAsync(putObjectRequest);
    }

    public async Task<GetObjectResponse> GetImageAsync(Guid id)
    {
        var getObjectRequest = new GetObjectRequest()
        {
            BucketName = BucketName,
            Key = $"images/{id}"
        };

        return await _s3.GetObjectAsync(getObjectRequest);
    }

    public async Task<DeleteObjectResponse> DeleteImageAsync(Guid id)
    {
        var deleteObjectRequest = new DeleteObjectRequest()
        {
            BucketName = BucketName,
            Key = $"images/{id}"
        };

        return await _s3.DeleteObjectAsync(deleteObjectRequest);
    }
}