<template>
  <CFooter class="px-4">
    <div class="waves">
      <div class="wave" id="wave1"></div>
      <div class="wave" id="wave2"></div>
      <div class="wave" id="wave3"></div>
      <div class="wave" id="wave4"></div>
    </div>
    <div>2024 SteamNexus</div>
    <div class="ms-auto">Crafted by iSpan 139 全端班 第三組</div>
  </CFooter>
</template>

<style scoped>
.footer {
  position: relative;
  margin-top: 120px;
}
.wave {
  position: absolute;
  top: -100px;
  left: 0;
  width: 100%;
  height: 100px;
  background: url(/img/wave.png);
  background-size: 1000px 100px;
}

.wave#wave1 {
  z-index: 1000;
  opacity: 1;
  bottom: 0;
  animation: animateWave 4s linear infinite;
}

.wave#wave2 {
  z-index: 999;
  opacity: 0.5;
  bottom: 10px;
  animation: animateWave_02 4s linear infinite;
}

.wave#wave3 {
  z-index: 1000;
  opacity: 0.2;
  bottom: 15px;
  animation: animateWave 3s linear infinite;
}

.wave#wave4 {
  z-index: 999;
  opacity: 0.7;
  bottom: 20px;
  animation: animateWave_02 3s linear infinite;
}

@keyframes animateWave {
  0% {
    background-position-x: 1000px;
  }
  100% {
    background-position-x: 0px;
  }
}

@keyframes animateWave_02 {
  0% {
    background-position-x: 0px;
  }
  100% {
    background-position-x: 1000px;
  }
}
</style>
