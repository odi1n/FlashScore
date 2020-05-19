using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlashScore;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace FlashScore_Unit_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task Get_All_Math()
        {
            FlashScoreApi flashApi = new FlashScoreApi();

            var allMatch = await flashApi.GetAllMatchesAsync(); 
            Debug.WriteLine("get all match");
        }

        [TestMethod]
        public async Task Get_All_Match_Info()
        {
            FlashScoreApi flashApi = new FlashScoreApi();

            var allMatch = await flashApi.GetAllMatchesAsync();
            Debug.WriteLine("get all match");

            await allMatch.GetInfoAsync(true, true, true, true);
            Debug.WriteLine("get all match info");
        }
    }
}
