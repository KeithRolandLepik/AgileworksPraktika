<template>
    <div class="navBar">
        <span class="link" @click="routeTo('/')">Home</span>
        <span class="link" @click="routeTo('/add')">Add</span>
        <span class="link" @click="routeTo('/login')">Login</span>
        <span class="link" @click="logOut()">LogOut</span>
    </div>
  </template>
  
  <script lang="ts">

  import useUsers from '@/api/useUsers';
import { defineComponent, ref, Ref} from 'vue';
  import { useRouter } from 'vue-router';

  export default defineComponent({
    setup() {
        var {logoutUser} = useUsers();
        var router = useRouter();
        var routeTo = async (route: string) => {
          router.push(route);
        }              
        var logOut = async () => {
            logoutUser();
        }      
      return {routeTo,logOut};
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

  .navBar {
      width:100%;
      height:50px;
      display:flex;
      flex-direction: row;
      justify-content: space-around;
      background-color: $gray1;
    .link{
        font-size: large;
        margin:auto;
    }
  }

  </style>
  