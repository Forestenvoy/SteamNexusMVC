<template>
  <div class="mt-5 container mb-2 text-white">
    <div class="position-absolute top-0 start-50 translate-middle-x animate__animated animate__fadeIn" :style="ImageBackground"></div>
    <h1 class="shadow-sm Name" :key="Name">{{Name}}</h1>
    <hr style="height: 2px; background-color:white; border: none;opacity: 1;"  class="mt-4 mb-5">
    <div class="row">
      <div class="col-4 p-2">
        <img :src="ImagePath" class="w-100 animate__animated animate__fadeIn" alt="">
        <p class="mt-3">{{Description}}</p>
      </div>
      <div class="col p-2">
        <h3 class="ms-3 animate__animated animate__fadeIn">歷史價格分析</h3>
        <div class="hello animate__animated animate__fadeIn" ref="chartdiv"></div>
      </div>
    </div>  
  </div>
  
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount ,onBeforeMount,nextTick,reactive,watch} from 'vue';
import * as am5 from '@amcharts/amcharts5';
import * as am5xy from '@amcharts/amcharts5/xy';
import Dark from '@amcharts/amcharts5/themes/Dark';

const apiUrl = import.meta.env.VITE_APP_API_BASE_URL


var AppId = ref('')
var Name = ref('')
var OriginalPrice = ref('')
var AgeRating = ref('')
var ReleaseDate = ref('')
var Publisher = ref('')
var Description = ref('')
var ImagePath = ref('')
var VideoPath = ref('')

var ImageBackground=reactive({
  'background-size': 'cover',
  width: '100%',
  height: '500px',
  'mask-image': 'linear-gradient(to bottom, rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0))',
  'z-index': '-1',
  'margin-top':'107px'
})

watch(ImagePath, (newPath) => {
  ImageBackground['background-image'] = `url(${newPath})`
}, { immediate: true })

let props = defineProps({
  gameId: Number
})

const chartdiv = ref(null);
let root = null;

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
      AppId.value = val.appId
      Name.value = val.name
      OriginalPrice.value = val.originalPrice
      AgeRating.value = val.ageRating
      ReleaseDate.value = val.releaseDate
      Publisher.value = val.publisher
      Description.value = val.description
      ImagePath.value = val.imagePath
      VideoPath.value = val.videoPath
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
  getData();
  console.log(Name)
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

  // Add legend
  
});

onBeforeUnmount(() => {
  if (root) {
    root.dispose();
  }
});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
@import 'animate.css';
.hello {
  width: 100%;
  height: 300px;
}

.Name{
  margin-top: 200px;
}
</style>
