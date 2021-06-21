<template>
  <router-view :key="routeKey" />
</template>

<script lang="ts">
import { defineComponent, watch, ref } from 'vue';
import useUsers from "./api/useUsers";

export default defineComponent({
  name: 'App',
  components: {},
  setup(){
    const{user} = useUsers();
    const routeKey = ref(0);
    watch([user], () => {
      if(user.value){
        routeKey.value = user.value.Id;
        localStorage.setItem('user', JSON.stringify(user.value))
        }
      });   
      if(user.value == undefined){
          if(localStorage.getItem('user')){
            user.value = JSON.parse(localStorage.getItem('user'));
          }
      }

      return{routeKey}
    }
  
});
</script>

<style lang="scss">
$gray: rgba(28, 28, 30);
$gray1: rgba(34, 34, 36);
$gray2: rgba(58, 58, 60);
$gray3: rgba(72, 72, 74);
$gray4: rgba(99, 99, 102);
$gray5: rgba(142, 142, 147);
#app {
  /* font-family: -apple-system, BlinkMacSystemFont, sans-serif; */
  font-family: 'Open-sans', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #ffffff;
  font-weight: 600;
  margin-top: 0px;
  height: 100vh;
  width: 100vw;
  overflow: hidden;
  font-size: x-large;
  background-color: $gray;
}
html {
  height: 100%;
}
* {
  -webkit-transition: all 0.2s ease-in-out;
  -moz-transition: all 0.2s ease-in-out;
  -ms-transition: all 0.2s ease-in-out;
  -o-transition: all 0.2s ease-in-out;
}
</style>
