using System.Text;
using Amazon.S3;
using Amazon.S3.Model;

var s3Client = new AmazonS3Client();

var getObjectRequest = new GetObjectRequest()
{
     BucketName = "myawscoursebucket",
     Key = "files/simple_text_file.txt"
};

var response = await s3Client.GetObjectAsync(getObjectRequest);

using var memoryStream = new MemoryStream();
await response.ResponseStream.CopyToAsync(memoryStream);

var text = Encoding.Default.GetString(memoryStream.ToArray());

Console.WriteLine(text);



// using var inputStream = new FileStream("./Screenshot_1.jpg", FileMode.Open, FileAccess.Read);
// using var inputStream = new FileStream("./some.txt", FileMode.Open, FileAccess.Read);

// var putObjectRequest1 = new PutObjectRequest()
// {
//     InputStream = inputStream,
//     BucketName = "myawscoursebucket",
//     Key = "images/profilePhoto.jpg",
//     ContentType = "image/jpg"
// };

// var putObjectRequest2 = new PutObjectRequest()
// {
//     InputStream = inputStream,
//     BucketName = "myawscoursebucket",
//     Key = "files/simple_text_file.txt",
//     ContentType = "text/csv"
// };

// await s3Client.PutObjectAsync(putObjectRequest1);
// await s3Client.PutObjectAsync(putObjectRequest2);
