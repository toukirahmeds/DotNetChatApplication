using System;
using System.IO;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using System.Net;
using System.Threading.Tasks;

namespace ChatMainServer{
    public static class S3BucketController{
       
        public async static Task<bool> S3UploadObject(string key, string filePath){
            Console.WriteLine("UPLOADING FILE: {0}", filePath);
            Console.WriteLine(key);
            PutObjectRequest request = new PutObjectRequest();
            FileStream fs = new FileStream( filePath, FileMode.Open);
            request.InputStream = fs;
            request.BucketName = Configs.S3BucketName;
            request.Key = key;
            try{
                PutObjectResponse response = await Configs.S3Client.PutObjectAsync(request);
                Console.WriteLine("FILE Has been uploaded UPLOADED");
            }catch(AmazonS3Exception e){
                Console.WriteLine("AmazonS3Exception Caught : {0}", e.Message);
            }finally{
                fs.Close();
            }
            return true;
            
        }


        public async static void S3CreateBucket(){
            ListBucketsResponse response = await Configs.S3Client.ListBucketsAsync();
            Console.WriteLine(response);
        }


        public async static void S3DownloadObject(string key, string directoryPath){
            Console.WriteLine("Downloading file with key '{0}' to {1}", key, directoryPath);
            GetObjectRequest request = new GetObjectRequest(){
                Key = key,
                BucketName = Configs.S3BucketName
            };
            try{
                GetObjectResponse response = await Configs.S3Client.GetObjectAsync(request);
                Stream responseStream = response.ResponseStream;
                StreamReader reader = new StreamReader(responseStream);
                Directory.CreateDirectory(directoryPath);
                File.WriteAllText(directoryPath + "/" + key, reader.ReadToEnd());
                Console.WriteLine("File Has been downloaded");
                reader.Close();
            }catch(AmazonS3Exception e){
                Console.WriteLine("AmazonS3Exception : {0}", e.Message);
            }
            
        }
    }
}