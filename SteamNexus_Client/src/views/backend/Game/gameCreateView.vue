<template>
  <div id="app">
    <h2>套用一個現成的流程</h2>
    <p>參考文件：<a href="https://hackmd.io/FFv0a5cBToOATP7uI5COMQ">https://hackmd.io/FFv0a5cBToOATP7uI5COMQ</a></p>

    <h3>範例：載入 VeeValidate 驗證套件</h3>
    <v-form @submit="onSubmit" v-slot="{ errors }">
      <div class="mb-3">
        <v-field
          id="email"
          name="email"
          type="email"
          class="form-control"
          :class="{ 'is-invalid': errors['email'] }"
          placeholder="請輸入 Email"
          rules="required|email"
          v-model="user.email"
        ></v-field>
        <error-message name="email" class="invalid-feedback"></error-message>
      </div>

      <div class="mb-3">
        <label for="name" class="form-label">姓名</label>
        <v-field id="name" name="姓名" type="text" class="form-control" placeholder="請輸入姓名"></v-field>
        <span class="invalid-feedback"></span>
      </div>

      <div class="mb-3">
        <label for="phone" class="form-label">電話</label>
        <v-field id="phone" name="電話" type="text" class="form-control" placeholder="請輸入電話"></v-field>
        <span class="invalid-feedback"></span>
      </div>

      <div class="mb-3">
        <label for="region" class="form-label">地區</label>
        <select id="region" name="地區" class="form-control">
          <option value="">請選擇地區</option>
          <option value="台北市">台北市</option>
          <option value="高雄市">高雄市</option>
        </select>
        <span class="invalid-feedback"></span>
      </div>

      <div class="mb-3">
        <label for="address" class="form-label">地址</label>
        <input id="address" name="地址" type="text" class="form-control" placeholder="請輸入地址">
        <span class="invalid-feedback"></span>
      </div>

      <button class="btn btn-primary" type="submit">Submit</button>
    </v-form>
  </div>
</template>

<script>
import { defineRule, configure, Form, Field, ErrorMessage } from 'vee-validate';
import { required, email } from '@vee-validate/rules';
import { loadLocaleFromURL, localize } from '@vee-validate/i18n';

export default {
  name: 'App',
  components: {
    VForm: Form,
    VField: Field,
    ErrorMessage: ErrorMessage
  },
  data() {
    return {
      user: {
        email: '',
        name: '',
        address: '',
        phone: ''
      }
    };
  },
  methods: {
    onSubmit() {
      console.log(this.user);
    },
    isPhone(value) {
      const phoneNumber = /^(09)[0-9]{8}$/;
      return phoneNumber.test(value) ? true : '需要正確的電話號碼';
    }
  },
  created() {
    defineRule('email', email);
    defineRule('required', required);
    loadLocaleFromURL('./zh_TW.json');
    configure({
      generateMessage: localize('zh_TW'),
      validateOnInput: true
    });
  }
};
</script>

<style scoped>
/* 在這裡添加相關的 CSS 樣式 */
</style>
