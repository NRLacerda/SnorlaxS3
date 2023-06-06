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
Console.WriteLine("------------------------------------------------------------------");
Console.WriteLine("Bem vindo ao SnorlaxS3, software para automatizar seus backups");
Console.WriteLine("------------------------------------------------------------------");


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
Console.WriteLine("------------------------------------------------------------------");
Console.WriteLine("ID   |                  Nome");
Console.WriteLine("------------------------------------------------------------------");

foreach (S3Bucket bucket in listResponse.Buckets)
{
    Console.WriteLine($"ID {indexBucket}   Bucket name {bucket.BucketName}");
    indexBucket++;
}
Console.WriteLine("------------------------------------------------------------------");
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
TransferUtilityUploadRequest request = new()
{
    BucketName = bucketName,
    Key = "acesskeys", //file name up in S3
    FilePath = "F:\\S3\\accesskeys.txt" //local file name
};
Console.WriteLine($"Deseja continuar a operação? Será feito o upload de arquivos no Bucket:  {bucketName}");
Console.WriteLine("Caso deseje continuar aperte a tecla Y, do contrário aperte ESC");
if (Console.ReadKey().Key == ConsoleKey.Y)
{
    try
    {
        utility.Upload(request); // Pusha os arquivos
        Console.WriteLine("\n Upload feito com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nHouve algum erro durante o upload. \n {ex} ");
    }
}
else
{
    Console.WriteLine("Operação Cancelada.");
}


//Esse read line está aqui pra impedir o app de fechar, ele não faz nada
Console.ReadLine();
