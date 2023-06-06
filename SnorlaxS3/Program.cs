using System;
using System.Threading.Tasks;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

// Specify your AWS credentials and region
var credentials = new Amazon.Runtime.BasicAWSCredentials("AKIA37OQ3BN2IFTTEWHT", "5AM32tJLojsINIOGJW4SRBiKDmOAYpbEKBth2CMC");
var region = RegionEndpoint.USEast1; // Replace YOUR_REGION with the desired region (e.g., RegionEndpoint.USWest2 for US West (Oregon))

// Create the S3 client
var s3Client = new AmazonS3Client(credentials, region);

Console.WriteLine("Bem vindo ao SnorlaxS3, software para automatizar seus backups");

var listResponse = await MyListBucketsAsync(s3Client);
string[] bucketNamesArray = new string[listResponse.Buckets.Count];

int index = 0;
foreach (S3Bucket bucket in listResponse.Buckets)
{
    bucketNamesArray[index] = bucket.BucketName;
    index++;
}

Console.WriteLine($"Quantidade de buckets: {listResponse.Buckets.Count}");
int indexBucket = 0;
foreach(S3Bucket bucket in listResponse.Buckets)
{
    Console.WriteLine($"ID {indexBucket}   Bucket name {bucket.BucketName}");
    indexBucket++;
}
Console.WriteLine("Escolha um bucket para fazer o upload de seus arquivos (use o ID):");
int bucketChoice = Convert.ToInt32(Console.ReadLine()); 
string bucketName = bucketNamesArray[bucketChoice];

static async Task<ListBucketsResponse> MyListBucketsAsync(IAmazonS3 s3Client)
{
    return await s3Client.ListBucketsAsync();
}

// Passa as config do Client para instanciar o Transferidor
TransferUtility utility = new TransferUtility(s3Client);
// Cria um upload Request
TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
request.BucketName = bucketName;
request.Key = "acesskeys"; //file name up in S3
request.FilePath = "F:\\S3\\accesskeys.txt"; //local file name
try
{
    utility.Upload(request); //commensing the transfer
    Console.WriteLine($"\nBucket selecionado -> {request.BucketName}");
    Console.WriteLine("Upload feito com sucesso!");
}
catch(Exception ex)
{
    Console.WriteLine($"\nHouve algum erro durante o upload. \n {ex} ");
}

//Esse read line está aqui para fins de teste, ele não faz nada
Console.ReadLine();
