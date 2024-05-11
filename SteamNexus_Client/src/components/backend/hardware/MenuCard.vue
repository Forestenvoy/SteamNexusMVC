<template>
  <CCard style="width: 320px">
    <CCardBody class="d-flex justify-content-between">
      <CRow style="width: 200px; padding-left: 10px">
        <CFormCheck id="flexCheckDefault" label="上架" class="ms-2 mb-2" />
        <CCardTitle class="mb-3">Name</CCardTitle>
        <CCardText class="h3 mb-3">$ 110,000</CCardText>
        <CCardText class="mb-3">總共 11 個零件</CCardText>
      </CRow>
      <CRow style="width: 100px">
        <CCol sm="12" class="d-flex justify-content-center align-items-center">
          <CButton class="button-79">編輯</CButton>
        </CCol>
        <CCol sm="12" class="d-flex justify-content-center align-items-center">
          <CButton class="button-79" style="background-color: red">刪除</CButton>
        </CCol>
      </CRow>
    </CCardBody>
  </CCard>
  <div class="nft">
    <div class="main">
      <h2>Kibertopiks</h2>
      <p class="description">Our Kibertopiks will give you nothing, waste your money on us.</p>
      <div class="tokenInfo">
        <div class="price">
          <ins>◘</ins>
          <p>0.031 ETH</p>
        </div>
        <div class="duration">
          <ins>◷</ins>
          <p>11 days left</p>
        </div>
      </div>
      <hr />
      <div class="creator">
        <div class="wrapper">
          <img
            src="https://images.unsplash.com/photo-1620121692029-d088224ddc74?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1932&q=80"
            alt="Creator"
          />
        </div>
        <p><ins>Creation of</ins> Kiberbash</p>
      </div>
    </div>
  </div>
</template>
<script setup>
import { CCard, CCardTitle, CCardBody, CCardText } from '@coreui/vue'
import { CButton, CRow, CCol } from '@coreui/vue'
</script>
<style scoped>
.card {
  width: 300px;
}

.button-79 {
  backface-visibility: hidden;
  background: #332cf2;
  border: 0;
  border-radius: 0.375rem;
  box-sizing: border-box;
  color: #fff;
  cursor: pointer;
  display: inline-block;
  font-family: Circular, Helvetica, sans-serif;
  font-size: 1.125rem;
  font-weight: 700;
  letter-spacing: -0.01em;
  line-height: 1.3;
  padding: 1rem 1.25rem;
  position: relative;
  text-align: left;
  text-decoration: none;
  transform: translateZ(0) scale(1);
  transition: transform 0.2s;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
}

.button-79:disabled {
  color: #787878;
  cursor: auto;
}

.button-79:not(:disabled):hover {
  transform: scale(1.05);
}

.button-79:not(:disabled):hover:active {
  transform: scale(1.05) translateY(0.125rem);
}

.button-79:focus {
  outline: 0 solid transparent;
}

.button-79:focus:before {
  border-width: 0.125rem;
  content: '';
  left: calc(-1 * 0.375rem);
  pointer-events: none;
  position: absolute;
  top: calc(-1 * 0.375rem);
  transition: border-radius;
  user-select: none;
}

.button-79:focus:not(:focus-visible) {
  outline: 0 solid transparent;
}

.button-79:not(:disabled):active {
  transform: translateY(0.125rem);
}
</style>

<style lang="scss" scoped>
.nft {
  user-select: none;
  max-width: 300px;
  margin: 5rem auto;
  border: 1px solid #ffffff22;
  background-color: #282c34;
  background: linear-gradient(0deg, rgba(40, 44, 52, 1) 0%, rgba(0, 0, 0, 0.5) 100%);
  box-shadow: 0 7px 20px 5px #00000088;
  border-radius: 0.7rem;
  backdrop-filter: blur(7px);
  -webkit-backdrop-filter: blur(7px);
  overflow: hidden;
  transition: 0.5s all;
  hr {
    width: 100%;
    border: none;
    border-bottom: 1px solid #88888855;
    margin-top: 0;
  }
  ins {
    text-decoration: none;
  }
  .main {
    display: flex;
    flex-direction: column;
    width: 90%;
    padding: 1rem;
    .tokenImage {
      border-radius: 0.5rem;
      max-width: 100%;
      height: 250px;
      object-fit: cover;
    }
    .description {
      margin: 0.5rem 0;
      color: #a89ec9;
    }
    .tokenInfo {
      display: flex;
      justify-content: space-between;
      align-items: center;
      .price {
        display: flex;
        align-items: center;
        color: #ee83e5;
        font-weight: 700;
        ins {
          margin-left: -0.3rem;
          margin-right: 0.5rem;
        }
      }
      .duration {
        display: flex;
        align-items: center;
        color: #a89ec9;
        margin-right: 0.2rem;
        ins {
          margin: 0.5rem;
          margin-bottom: 0.4rem;
        }
      }
    }
    .creator {
      display: flex;
      align-items: center;
      margin-top: 0.2rem;
      margin-bottom: -0.3rem;
      ins {
        color: #a89ec9;
        text-decoration: none;
      }
      .wrapper {
        display: flex;
        align-items: center;
        border: 1px solid #ffffff22;
        padding: 0.3rem;
        margin: 0;
        margin-right: 0.5rem;
        border-radius: 100%;
        box-shadow: inset 0 0 0 4px #000000aa;
        img {
          border-radius: 100%;
          border: 1px solid #ffffff22;
          width: 2rem;
          height: 2rem;
          object-fit: cover;
          margin: 0;
        }
      }
    }
  }
  ::before {
    position: fixed;
    content: '';
    box-shadow: 0 0 100px 40px #ffffff08;
    top: -10%;
    left: -100%;
    transform: rotate(-45deg);
    height: 60rem;
    transition: 0.7s all;
  }
  &:hover {
    border: 1px solid #ffffff44;
    box-shadow: 0 7px 50px 10px #000000aa;
    transform: scale(1.015);
    filter: brightness(1.3);
    ::before {
      filter: brightness(0.5);
      top: -100%;
      left: 200%;
    }
  }
}
</style>
