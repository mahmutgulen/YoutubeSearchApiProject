using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YoutubeSearchApıProject.Entities;
using static YoutubeSearchApıProject.Entities.YoutubeSearchDto;

namespace YoutubeSearchApıProject.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            string apiKey = "AIzaSyAX6Lu92n7cNR-Y645HSj-gVMTl076Hgt0"; 
            string url = "https://www.googleapis.com/youtube/v3/";

            string request = $"{url}search?part=snippet&key={apiKey}&type=video&q={search}";
            List<ResponseDto> list = new List<ResponseDto>();

            //apiRequest

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{request}");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<YoutubeSearchDto.Root>(apiResponse);

            if (data.items == null)
            {
                list.Add(new ResponseDto
                {
                    ImgHeight = 0,
                    ImgWidth = 0,
                    ImgUrl = "",
                    VideoDescription = "",
                    VideoTitle = "",
                    VideoUrl = "",
                });
                return View(list);
            }

            foreach (var item in data.items)
            {
                list.Add(new ResponseDto
                {
                    ImgHeight = item.snippet.thumbnails.high.height,
                    ImgWidth = item.snippet.thumbnails.high.width,
                    ImgUrl = item.snippet.thumbnails.high.url,
                    VideoDescription = item.snippet.description,
                    VideoTitle = item.snippet.title,
                    VideoUrl = "https://www.youtube.com/watch?v=" + item.id.videoId,
                });
            }
            return View(list);
        }

    }
}
