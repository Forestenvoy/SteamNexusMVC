<template>
  <Form @submit="onSubmit">
    <div class="formBox">
      <label class="text-center" for="AppId">AppId</label>
      <Field  id="AppId" name="AppId" class="form-control text-center" type="number" rules="required|numeric" v-model="AppId" />
      <ErrorMessage class="text-danger " name="AppId" />
    </div>
    <div class="formBox">
      <label class="text-center" for="Name">遊戲名稱</label>
      <Field name="Name" class="form-control text-center" type="text"  rules="required" v-model="Name"/>
      <ErrorMessage class="text-danger" name="Name" />
    </div>
    <div class="formBox">
      <label class="text-center" for="OriginalPrice">原始價格</label>
      <Field name="OriginalPrice" class="form-control text-center" type="number" rules="required" v-model="OriginalPrice"/>
      <ErrorMessage class="text-danger" name="OriginalPrice" />
    </div>
    <div class="formBox">
      <label class="text-center" for="AgeRating">遊戲分級</label>
      <Field name="AgeRating" class="form-control text-center" type="text" rules="required" v-model="AgeRating" aria-placeholder="18+"/>
      <ErrorMessage class="text-danger" name="AgeRating" />
    </div>
    <div class="formBox">
      <label class="text-center" for="ReleaseDate">上市日期</label>
      <Field name="ReleaseDate" class="form-control text-center" type="date" rules="" v-model="ReleaseDate"/>
      <ErrorMessage class="text-danger" name="ReleaseDate" />
    </div>
    <div class="formBox">
      <label class="text-center" for="Publisher">開發商</label>
      <Field name="Publisher" class="form-control text-center" type="text" rules="" v-model="Publisher"/>
      <ErrorMessage class="text-danger" name="Publisher" />
    </div>
    <div class="formBox">
      <label class="text-center" for="Description">遊戲介紹</label>
      <Field name="Description" class="form-control text-center" type="text" rules="" v-model="Description"  />
      <ErrorMessage class="text-danger" name="Description" />
    </div>
      <div class="formBox">
      <label class="text-center" for="ImagePath">遊戲圖片</label>
      <img v-bind:src="imagesrc" id="imgPreview" title="上無內容" style="width:250px;" /><br>
      <Field name="ImagePath" class="form-control text-center" type="text" rules="" @change="ImageChange" v-model="ImagePath"/>
      <ErrorMessage class="text-danger" name="ImagePath" />
    </div>
    <div class="formBox">
      <label class="text-center" for="VideoPath">遊戲影片</label>
      <video v-bind:src="videosrc" id="videoPreview" width="250" controls autoplay muted></video><br>
      <Field name="VideoPath" class="form-control text-center" type="text" rules="" @change="VideoChange" v-model="VideoPath"/>
      <ErrorMessage class="text-danger" name="VideoPath" />
    </div>
    <button>Submit</button>
  </Form>
</template>

<script setup>
import { defineRule, Form, Field, ErrorMessage, configure } from 'vee-validate';
import { required, between, confirmed, numeric} from '@vee-validate/rules';
import { localize } from '@vee-validate/i18n';
import zh_TW from'@/components/backend/Game/zh_TW.json'
import { ref, onMounted,defineEmits } from 'vue'

// define global rules
defineRule('required', required);
defineRule('between', between);
defineRule('confirmed', confirmed);
defineRule('numeric', numeric);
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL
var imagesrc = ref('http://localhost:5173/public/img/noImage.png')
var videosrc = ref('#')

var AppId=ref("")
var Name=ref("")
var OriginalPrice=ref("")
var AgeRating=ref("")
var ReleaseDate=ref("")
var Publisher=ref("")
var Description=ref("")
var ImagePath=ref("")
var VideoPath=ref("")

function ImageChange(event) {
    if (event.target.value != "") {
        var img = new Image();
        img.onload = function () {
            imagesrc=event.target.value
        };
        img.onerror = function () {
            // 如果網址有效但沒有圖片，顯示預設圖片
            $("#imgPreview").attr("src", "/img/noImage.png");
        };
        img.src = event.target.value; // 設置圖片網址來檢查它是否有效
    }
    else {
        $("#imgPreview").attr("src", "/img/noImage.png");
    }
    
}

function VideoChange(event) {
    if (event.target.value != "") {
        videosrc=event.target.value
    } 
}

function onSubmit(event) {
  
  $.ajax({
    type: "POST",
    contentType: "application/json",
    data: JSON.stringify({
        appId: AppId.value,
        name: Name.value,
        originalPrice:OriginalPrice.value,
        ageRating: AgeRating.value,
        releaseDate: ReleaseDate.value===""?null:ReleaseDate.value  ,
        publisher: Publisher.value,
        description: Description.value,
        imagePath: ImagePath.value,
        videoPath: VideoPath.value
    }),
    url: `${apiUrl}/api/GamesManagement/PostCreatPartialToDB`
}).done(data => {
        $("#systemLoading").hide();
        $("#System").html(data);
    })
    .fail(data => {
        $("#systemLoading").hide();
        $("#System").html(data);
    });
    
}

// function submitDataToDb(){
// $.ajax({
//     type: "POST",
//     contentType: "application/json",
//     data: JSON.stringify({
//         appId: AppId,
//         name: Name,
//         originalPrice:OriginalPrice,
//         ageRating: AgeRating,
//         releaseDate: ReleaseDate,
//         publisher: Publisher,
//         description: Description,
//         imagePath: ImagePath,
//         videoPath: VideoPath
//     }),
//     url: `${apiUrl}/api/GamesManagement/PostCreatPartialToDB`
// }).done(data => {
//         $("#systemLoading").hide();
//         $("#System").html(data);
//     })
//     .fail(data => {
//         $("#systemLoading").hide();
//         $("#System").html(data);
//     });
    
// }




defineExpose({AppId,Name,OriginalPrice,AgeRating,ReleaseDate,Publisher,Description,ImagePath,VideoPath})

configure({
  generateMessage: localize('zh_TW', {
    names: {
      Name: '遊戲名稱',
      OriginalPrice:'原始價格',
      AgeRating:'遊戲分級',
      ReleaseDate:'上市日期',
      Publisher:'開發商',
      Description:'遊戲介紹',
      ImagePath:'遊戲圖片',
      VideoPath:'遊戲影片'
    },
    messages: zh_TW.messages
  })
});

</script>

<style scoped>
@import 'bootstrap/dist/css/bootstrap.min.css';
span,
button {
  display: block;
  margin: 10px 0;
}

label {
  display: block;
  margin-bottom: 5px;
}

.formBox{
  margin-bottom: 20px;
}
</style>

<!-- var AppId=ref("")
var Name=ref("")
var OriginalPrice=ref("")
var AgeRating=ref("")
var ReleaseDate=ref("")
var Publisher=ref("")
var Description=ref("")
var ImagePath=ref("")
var VideoPath=ref("") 
AppId,Name,OriginalPrice,AgeRating,ReleaseDate,Publisher,Description,ImagePath,VideoPath-->
