<template>
  <CRow class="block">
    <CCol xs="12" md="6" class="text-center"
      ><h3>我時間寶貴，我不想一步一步選擇</h3>
      <a href="#menuSlider" class="button-80">觀看現成的菜單</a>
    </CCol>
    <CCol xs="12" md="6" class="text-center"
      ><h3>我等不及了，快點開始吧</h3>
      <button class="button-80" @click="next">GO</button>
    </CCol>
  </CRow>
</template>

<script setup>
// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

const next = () => {
  builderStore.next()
}
</script>

<style scoped>
/* 自定義 */
.block {
  padding: 50px 0 50px 0;
}

h3 {
  margin-bottom: 50px;
}

/* CSS */
.button-80 {
  background: #fff;
  backface-visibility: hidden;
  border-radius: 0.375rem;
  border-style: solid;
  border-width: 0.125rem;
  box-sizing: border-box;
  color: #212121;
  cursor: pointer;
  display: inline-block;
  font-family: Circular, Helvetica, sans-serif;
  font-size: 1.125rem;
  font-weight: 700;
  letter-spacing: -0.01em;
  line-height: 1.3;
  padding: 0.875rem 1.125rem;
  position: relative;
  text-align: left;
  text-decoration: none;
  transform: translateZ(0) scale(1);
  transition: transform 0.2s;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
}

.button-80:not(:disabled):hover {
  transform: scale(1.05);
}

.button-80:not(:disabled):hover:active {
  transform: scale(1.05) translateY(0.125rem);
}

.button-80:focus {
  outline: 0 solid transparent;
}

.button-80:focus:before {
  content: '';
  left: calc(-1 * 0.375rem);
  pointer-events: none;
  position: absolute;
  top: calc(-1 * 0.375rem);
  transition: border-radius;
  user-select: none;
}

.button-80:focus:not(:focus-visible) {
  outline: 0 solid transparent;
}

.button-80:focus:not(:focus-visible):before {
  border-width: 0;
}

.button-80:not(:disabled):active {
  transform: translateY(0.125rem);
}
</style>
