<template>
  <NavBar/>
    <div class="page">
      <div class="header">
        <span class="headerText">Authenticate</span>
      </div>
      <div class="content">
        <Suspense>
          <Authenticate />
        </Suspense>
      </div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue';
  import Authenticate from '../components/Authenticate/Authenticate.vue';import NavBar from '@/components/NavBar/NavBar.vue';
  export default defineComponent({
    components: {
      Authenticate,NavBar
    },
    setup() {
      return {};
    },
  });
  </script>
  
  <style scoped lang="scss">
  $gray: rgba(28, 28, 30);
  $gray1: rgba(34, 34, 36);
  $gray2: rgba(58, 58, 60);
  $gray3: rgba(72, 72, 74);
  $gray4: rgba(99, 99, 102);
  $gray5: rgba(142, 142, 147);
  .page {
    height: 100%;
    width: 100%;
    overflow-x: hidden;
    display: flex;
    justify-content: center;
    flex-direction: column;
    padding: 50px;
    background: $gray;
    // background: #C9D6FF;  /* fallback for old browsers */
    // background: -webkit-linear-gradient(to right, #E2E2E2, #dee6fd);  /* Chrome 10-25, Safari 5.1-6 */
    // background: linear-gradient(160deg, #E2E2E2, #e6ecff); /* W3C, IE 10+/ Edge, Firefox 16+, Chrome 26+, Opera 12+, Safari 7+ */
    .header {
      width: 50%;
      display: flex;
      justify-content: center;
      margin: auto;
  
      .headerText {
        font-size: xx-large;
        margin: auto;
      }
    }
    .content {
      height: 100%;
      width: 50%;
      display: flex;
      margin: auto;
    }
  }
  </style>
  