<template>
  <div>我是遊戲頁面</div>
  <div>ID : {{ props.gameId }}</div>
  <div class="hello" ref="chartdiv"></div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue';
import * as am5 from '@amcharts/amcharts5';
import * as am5xy from '@amcharts/amcharts5/xy';
import am5themes_Animated from '@amcharts/amcharts5/themes/Animated';

let props = defineProps({
  gameId: Number
})

const chartdiv = ref(null);
let root = null;

onMounted(() => {
  root = am5.Root.new(chartdiv.value);
  root.setThemes([am5themes_Animated.new(root)]);

  let chart = root.container.children.push(
    am5xy.XYChart.new(root, {
      panY: false,
      layout: root.verticalLayout,
    })
  );

  // Define data
  let data = [
    { category: 'Research', value1: 1000, value2: 588 },
    { category: 'Marketing', value1: 1200, value2: 1800 },
    { category: 'Sales', value1: 850, value2: 1230 },
  ];

  // Create Y-axis
  let yAxis = chart.yAxes.push(
    am5xy.ValueAxis.new(root, {
      renderer: am5xy.AxisRendererY.new(root, {}),
    })
  );

  // Create X-Axis
  let xAxis = chart.xAxes.push(
    am5xy.CategoryAxis.new(root, {
      renderer: am5xy.AxisRendererX.new(root, {}),
      categoryField: 'category',
    })
  );
  xAxis.data.setAll(data);

  // Create series
  let series1 = chart.series.push(
    am5xy.ColumnSeries.new(root, {
      name: 'Series',
      xAxis: xAxis,
      yAxis: yAxis,
      valueYField: 'value1',
      categoryXField: 'category',
    })
  );
  series1.data.setAll(data);

  let series2 = chart.series.push(
    am5xy.ColumnSeries.new(root, {
      name: 'Series',
      xAxis: xAxis,
      yAxis: yAxis,
      valueYField: 'value2',
      categoryXField: 'category',
    })
  );
  series2.data.setAll(data);

  // Add legend
  let legend = chart.children.push(am5.Legend.new(root, {}));
  legend.data.setAll(chart.series.values);

  // Add cursor
  chart.set('cursor', am5xy.XYCursor.new(root, {}));
});

onBeforeUnmount(() => {
  if (root) {
    root.dispose();
  }
});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.hello {
  width: 100%;
  height: 500px;
}
</style>
