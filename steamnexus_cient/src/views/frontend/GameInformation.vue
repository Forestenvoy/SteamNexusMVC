<template>
  <div class="mt-5 container mb-2 text-white">
    <div class="position-absolute top-0 start-50 translate-middle-x" :style="ImageBackground"></div>
    <div style="margin-top: 200px;">
       <div class="fs-5 row">
      <div class="col-4 leftbox">
        <img :src="ImagePath" class="w-100 animate__animated animate__fadeIn" alt="">
        <div class="p-3">
          <p class=" text-secondary">{{Description}}</p>
          <div v-if="tagShow" class="mt-3 tagShow">
          <h7>熱門標籤：</h7>
          <button v-for="tag in tagData" class="badge" :key="tag" @click="tagclick">{{tag}}</button>
        </div>
        <div v-if="!tagShow" class="mt-3 tagShow" :key="tagShow">
          <h7>熱門標籤： </h7>
          <button v-for="tag in tagDataOpen" class="badge" :key="tag" @click="tagclick">{{tag}}</button>
        </div>
        <button class="badge" @click="moreclick(tagShow)">...</button><br>
        <span >名稱： {{Name}}</span><br>
        <span >原始價格： {{OriginalPrice==0?"免費":`NT.${OriginalPrice}`}}</span><br>
        <span >所有評論： {{Comment}}（ {{CommentNum}}人評論 ）</span><br>
        <span>開發商： {{Publisher}}</span><br>
        <span >發行日期： {{ReleaseDate}}</span><br>
        <img :src="AgeRating=='18+'?'https://www.gamerating.org.tw/Content/img/index_icon_05.jpg':AgeRating=='15+'?'https://www.gamerating.org.tw/Content/img/index_icon_04.jpg':AgeRating=='12+'?'https://www.gamerating.org.tw/Content/img/index_icon_03.jpg':AgeRating=='6+'?'https://www.gamerating.org.tw/Content/img/index_icon_02.jpg':'https://www.gamerating.org.tw/Content/img/index_icon_01.jpg'" alt="" class="w-25 mt-3 me-3"><span >遊戲分級： {{AgeRating}}</span><br>
        <div>
          <table class="table table-dark table-hover mt-3">
            <thead>
              <tr>
                <th>語言支援</th>
                <th class="text-center">介面</th>
                <th class="text-center">完整語言</th>
                <th class="text-center">字幕</th>
              </tr>
              <tr v-for="LT in LanguageTable" :key="LT.GameLanguageId">
                <td >{{ LT.name }}</td>
                <td class="text-center">{{ LT.support >= 1 ? '√' : '' }}</td>
                <td class="text-center">{{ LT.support == 2 ? '√' :LT.support == 1 ? '√' : '' }}</td>
                <td class="text-center">{{ LT.support == 3 ? '√' :LT.support == 1 ? '√': '' }}</td>
              </tr>
              </thead>
            </table>
        </div>
        </div>
      </div>
      <div class="col-8 p-1 ">
        <h1 class="shadow-sm ms-4" :key="Name">{{Name}}</h1>
        <hr style="height: 2px; background-color:white; border: none;opacity: 1;"  class="mt-3 mb-4 ms-3">
        <div class="d-flex justify-content-between ">
          <h3 class="ms-4 fs-1" data-aos="fade-in">歷史價格分析</h3>
          <div class="d-flex ">
            <h3 data-aos="fade-in">{{OriginalPrice==0?"免費":`NT.${OriginalPrice}`}}</h3>
            <a :href="steamWeb" target="_blank" :title="steamWebtitle"><button type="button" class="btn btn-primary ms-3">前往Steam購買</button></a>
          </div>  
        </div>
        <div>

        </div>
        <div class="hello animate__animated animate__fadeIn" ref="chartdiv"></div>
        <div class="d-flex p-2 mt-2">
          <div class="leftbox p-3 rounded m-1 " style="width: 50%;">
            <span class="fw-bold text-white fs-6">最低配備：</span><br>
            <div v-if='MinOriCpu!=null'>
              <span class="text-white">Cpu：</span>
              <span>{{MinOriCpu}}</span>
            </div>
            <div v-if='MinOriGpu!=null'>
              <span class="text-white">Gpu：</span>
              <span>{{MinOriGpu}}</span>
            </div>
            <div v-if='MinOriRam!=null'>
              <span class="text-white">記憶體：</span>
              <span>{{MinOriRam}}</span>
            </div>
            <div v-if='MinStorage!=null'>
              <span class="text-white">儲存空間：</span>
              <span>{{MinStorage}}</span>
            </div>
            <div v-if='MinOS!=null'>
              <span class="text-white">Windows系統：</span>
              <span>{{MinOS}}</span>
            </div>
            <div v-if='MinNetwork!=null'>
              <span class="text-white">網路需求：</span>
              <span>{{MinNetwork}}</span>
            </div>
            <div v-if='MinDirectX!=null'>
              <span class="text-white">DirectX：</span>
              <span>{{MinDirectX}}</span>
            </div>
            <div v-if='MinAudio!=null'>
              <span class="text-white">音效：</span>
              <span>{{MinAudio}}</span>
            </div>
            <div v-if='MinNote!=null'>
              <span class="text-white">備註：</span>
              <span>{{MinNote}}</span>
            </div>
          </div>
          <div class="leftbox p-3 rounded m-1 " style="width: 50%;">
            <span class="fw-bold text-white fs-6">最高配備：</span><br>
            <div v-if='RecOriCpu!=null'>
              <span class="text-white">Cpu：</span>
              <span>{{RecOriCpu}}</span>
            </div>
            <div v-if='RecOriGpu!=null'>
              <span class="text-white">Gpu：</span>
              <span>{{RecOriGpu}}</span>
            </div>
            <div v-if='RecOriRam!=null'>
              <span class="text-white">記憶體：</span>
              <span>{{RecOriRam}}</span>
            </div>
            <div v-if='RecStorage!=null'>
              <span class="text-white">儲存空間：</span>
              <span>{{RecStorage}}</span>
            </div>
            <div v-if='RecOS!=null'>
              <span class="text-white">Windows系統：</span>
              <span>{{RecOS}}</span>
            </div>
            <div v-if='RecNetwork!=null'>
              <span class="text-white">網路需求：</span>
              <span>{{RecNetwork}}</span>
            </div>
            <div v-if='RecDirectX!=null'>
              <span class="text-white">DirectX：</span>
              <span>{{RecDirectX}}</span>
            </div>
            <div v-if='RecAudio!=null'>
              <span class="text-white">音效：</span>
              <span>{{RecAudio}}</span>
            </div>
            <div v-if='RecNote!=null'>
              <span class="text-white">備註：</span>
              <span>{{RecNote}}</span>
            </div>
          </div>
        </div>
        <!-- 推薦遊戲 -->
        <Carousel  :items-to-show="3" v-bind="settings" >
          <Slide class="slide" v-for="slide in 10" :key="slide">
            <div class="carousel__item">
              <img width=" 100%" src="https://cdn.cloudflare.steamstatic.com/steam/apps/570/header.jpg?t=1710346933" alt="">
              <span>123</span>
            </div>
            
            
          </Slide>
          <template #addons>
            <Navigation class="bg-danger" />
          </template>
        </Carousel>
        <!-- carousel -->
      </div>
    </div>  
  </div>
    </div>
</template>

<script setup>
import  { ref, onMounted, onBeforeUnmount,reactive,watch , defineComponent } from 'vue';
import * as am5 from '@amcharts/amcharts5';
import * as am5xy from '@amcharts/amcharts5/xy';
import Dark from '@amcharts/amcharts5/themes/Dark';
import AOS from 'aos'

const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

var AppId = ref('')
var Name = ref('')
var OriginalPrice = ref('')
var CurrentPrice = ref('')
var AgeRating = ref('')
var ReleaseDate = ref('')
var Publisher = ref('')
var Description = ref('')
var ImagePath = ref('')
var VideoPath = ref('')
var Comment=ref('')
var CommentNum=ref('')
var steamWeb = ref("")
var steamWebtitle=ref("")

var MinCPUId=ref("")
var MinGPUId=ref("")
var MinRAM=ref("")
var MinOS=ref("")
var MinDirectX=ref("")
var MinNetwork=ref("")
var MinStorage=ref("")
var MinAudio=ref("")
var MinNote=ref("")
var MinOriCpu=ref("")
var MinOriGpu=ref("")
var MinOriRam=ref("")

var RecCPUId=ref("")
var RecGPUId=ref("")
var RecRAM=ref("")
var RecOS=ref("")
var RecDirectX=ref("")
var RecNetwork=ref("")
var RecStorage=ref("")
var RecAudio=ref("")
var RecNote=ref("")
var RecOriCpu=ref("")
var RecOriGpu=ref("")
var RecOriRam=ref("")

var tagData=ref([])
var tagDataOpen=ref([])
var tagShow=ref(true)
var LanguageTable=ref([])

import { Carousel, Navigation, Slide } from 'vue3-carousel'
import 'vue3-carousel/dist/carousel.css'
defineComponent({
  name: 'Breakpoints',
  components: {
    Carousel,
    Slide,
    Navigation,
  },
  data: () => ({
    // carousel settings
    settings: {
      itemsToShow: 1,
      snapAlign: 'start',
    },
  }),
})

var ImageBackground=reactive({
  'background-size': 'cover',
  width: '100%',
  height: '500px',
  'mask-image': 'linear-gradient(to bottom, rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0))',
  'z-index': '-1',
  'margin-top':'45px'
})

watch(ImagePath, (newPath) => {
  ImageBackground['background-image'] = `url(${newPath})`
}, { immediate: true })

let props = defineProps({
  gameId: Number
})

const chartdiv = ref(null);
let root = null;

function tagclick(event){
  console.log(event.target.innerText)
}

function moreclick(){
  tagShow.value=!tagShow.value
}
//拿取遊戲資料
function getData(){
  fetch(`${apiUrl}/api/GamesManagement/GetEditJSON?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      console.log(val);
      AppId.value = val.appId
      Name.value = val.name
      OriginalPrice.value = val.originalPrice
      AgeRating.value = val.ageRating
      ReleaseDate.value = val.releaseDate
      Publisher.value = val.publisher
      Description.value = val.description
      ImagePath.value = val.imagePath
      VideoPath.value = val.videoPath
      Comment.value=val.comment
      CommentNum.value=val.commentNum
      CurrentPrice.value=val.currentPrice
      steamWeb.value=`https://store.steampowered.com/app/${val.appId}`
      steamWebtitle.value=`前往${val.name}的Steam網頁`
    })
    .catch((error) => {
      alert(error)
    })
    .finally(() => {
      // 异步操作完成后启用按钮
      $(this).prop('disabled', false)
    })
}

function GetTagGroup(){
  fetch(`${apiUrl}/api/GamesManagement/GetTagGroup?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      console.log(val);
      for (var i = 0; i < 5; i++){
        tagData.value.push(val[i])
      }
      for (var i = 0; i < val.length; i++){
        tagDataOpen.value.push(val[i])
      }
    })
    .catch((error) => {
      alert(error)
    })
    .finally(() => {
      // 异步操作完成后启用按钮
      $(this).prop('disabled', false)
    })
}

function GetLanguageGroup(){
  fetch(`${apiUrl}/api/GamesManagement/GetLanguageGroup?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      for (var i = 0; i < val.length; i++){
        LanguageTable.value.push(val[i])
      }
    })
    .catch((error) => {
      alert(error)
    })
    .finally(() => {
      // 异步操作完成后启用按钮
      $(this).prop('disabled', false)
    })
}

function getMinReqData(){
  fetch(`${apiUrl}/api/GamesManagement/GetMinReqData?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      MinCPUId.value=val.cpuId
      MinGPUId.value=val.gpuId
      MinRAM.value=val.ram
      MinOS.value=val.os
      MinDirectX.value=val.directX
      MinNetwork.value=val.network
      MinStorage.value=val.storage
      MinAudio.value=val.audio
      MinNote.value=val.note
      MinOriCpu.value=val.oriCpu
      MinOriGpu.value=val.oriGpu
      MinOriRam.value=val.oriRam
      console.log(val);
    })
    .catch((error) => {
      alert(error)
    })
    .finally(() => {
      // 异步操作完成后启用按钮
      $(this).prop('disabled', false)
    })
}

function getRecReqData(){
  fetch(`${apiUrl}/api/GamesManagement/GetRecReqData?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then((val) => {
      RecCPUId.value=val.cpuId
      RecGPUId.value=val.gpuId
      RecRAM.value=val.ram
      RecOS.value=val.os
      RecDirectX.value=val.directX
      RecNetwork.value=val.network
      RecStorage.value=val.storage
      RecAudio.value=val.audio
      RecNote.value=val.note
      RecOriCpu.value=val.oriCpu
      RecOriGpu.value=val.oriGpu
      RecOriRam.value=val.oriRam
      console.log(val);
    })
    .catch((error) => {
      alert(error)
    })
    .finally(() => {
      // 异步操作完成后启用按钮
      $(this).prop('disabled', false)
    })
}

onMounted(() => {
  AOS.init()
  getData();
  GetTagGroup();
  GetLanguageGroup();
  getMinReqData();
  getRecReqData();

  fetch(`${apiUrl}/api/GamesManagement/GetLineChartData?id=${props.gameId}`, {
    method: 'GET'
  })
    .then((response) => {
      // 確保請求是否成功
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      // 解析 html
      return response.json()
    })
    .then(data => {
    // 将日期字符串转换为Date对象
    data.forEach(function(item) {
      item.date = new Date(item.date).getTime();
    });

    root = am5.Root.new(chartdiv.value);
    root.setThemes([Dark.new(root)]);

    let chart = root.container.children.push(
      am5xy.XYChart.new(root, {
        panY: false,
        layout: root.verticalLayout,
      })
    );
  
  // Create Y-axis
  let yAxis = chart.yAxes.push(
    am5xy.ValueAxis.new(root, {
      renderer: am5xy.AxisRendererY.new(root, {}),
    })
  );
  

  // Create X-Axis
let xAxis = chart.xAxes.push(
  am5xy.DateAxis.new(root, {
    baseInterval: { timeUnit: "day", count: 1 },
    renderer: am5xy.AxisRendererX.new(root, {})
  })
);

xAxis.get("renderer").labels.template.setAll({
  fill: root.interfaceColors.get("alternativeText")
});

xAxis.setAll({
  background: am5.Rectangle.new(root, {
    color:"#ff5755",
    fill: root.interfaceColors.get("alternativeBackground"),
    fillOpacity: 0.7
  })
});

xAxis.get("dateFormats")["day"] = "MM/dd";
xAxis.get("periodChangeDateFormats")["day"] = "MMM";

  // Create series
  let series1 = chart.series.push(
    am5xy.StepLineSeries.new(root, {
      name: 'Series',
      xAxis: xAxis,
      yAxis: yAxis,
      valueYField: 'price',
      valueXField: 'date',
      tooltip: am5.Tooltip.new(root, {}),
      fill: am5.color(0xff5755),
      stroke: am5.color(0xff5755)
    })
  );
  series1.data.setAll(data);

  //設定點點樣式
  series1.bullets.push(function() {
    return am5.Bullet.new(root, {
      sprite: am5.Circle.new(root, {
        radius: 5,
        fill: series1.get("fill")
      })
    });
  });

  //設定線條粗度
  series1.strokes.template.set("strokeWidth", 1);

  //設定框框內容
  series1.get("tooltip").label.set("text", "[bold]{name}[/]\n{valueX.formatDate()}\nNT.{valueY}")
  series1.data.setAll(data);

  let legend = chart.children.push(am5.Legend.new(root, {}));
  legend.data.setAll(chart.series.values);

  // Add cursor
  chart.set('cursor', am5xy.XYCursor.new(root, {}));
  console.log(data);
    })
    .catch((error) => {
      alert(error)
    })
});

onBeforeUnmount(() => {
  if (root) {
    root.dispose();
  }
});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.carousel__item {
  padding: 20px;
  min-height: 300px;
  width: 100%;
  background-color: #0f1c27;
  color: var(--vc-clr-white);
  font-size: 20px;
  border-radius: 8px;

  align-items: center;
}

.carousel__slide {
  width: 100%;
  padding: 10px;
}

.carousel__prev,
.carousel__next {
  box-sizing: content-box;
  border: 5px solid white;
  
}

.rounded span{
  color: gray
}
.leftbox{
 background: #0f1c27;  /* fallback for old browsers */

}
.hello {
  width: 100%;
  height: 300px;
}


.badge{
  font-size: 13px;
  font-weight: 300;
  border: 2px solid gray;
  margin: 2px;
  background-color: rgba(0, 0, 0, 0.0);
}

.tagShow{
  display: inline;
}

h7{
  font-weight:500;
}
h3{
  font-weight:bold;
}
p{
  font-weight:normal;
}

.row > * {
  padding-left: 0;
  padding-right: 0;
}
</style>
