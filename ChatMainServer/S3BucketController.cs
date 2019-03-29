using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace ChatMainServer{
    public static class S3BucketController{
       
        public static void S3Upload(){
            // PutBucketRequest request = new PutBucketRequest();
            // request.BucketName = "toukir-bucket";
            // Configs.S3Client.PutBucketAsync(request);
            // return false;
            // Configs.S3Client.up
        }


        public async static void S3CreateBucket(){
            ListBucketsResponse response = await Configs.S3Client.ListBucketsAsync();
            Console.WriteLine(response);
        }
    }
}