<template>
    <div class="page">
      <div class="container">
        <div class="header">Login</div>
        
        <div class="inputContainer">
          <span class="label">Username</span>
          <input type="text" class="input" v-model="userRequest.UserName">
          
          <span class="label">Password</span>  
          <input type="password" class="input" v-model="userRequest.Password">
        </div>
        
        <div class="action">
          <div class="buttonGroup">
            <button class="button" @click="loginUser()">Login</button>
            <button class="button" @click="register()">Register</button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script lang="ts">

  import useUsers from '@/api/useUsers';
  import UserRequest from '@/interfaces/UserRequest';
  import { defineComponent, ref, Ref} from 'vue';

  import { useRouter } from 'vue-router';

  export default defineComponent({
    setup() {
      const userRequest: Ref<UserRequest> = ref({});
      var { registerUser,authenticateUser} = useUsers();
      const route = useRouter();


      var loginUser = async () => {
        await authenticateUser(userRequest.value);
        route.push("/");
      }      
      var register = async () => {  
        await registerUser(userRequest.value);
      }
      return {userRequest,register,loginUser};
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
    
    .container{
      height:70%;
      width:100%;
      display:flex;
      flex-direction: column;
      background: $gray1;
      justify-items: flex-start;

      .header{
        flex:1;
        margin:0 auto;
        margin-top:20px;
      }

      .inputContainer{
        flex:4;
        display:flex;
        flex-direction: column;
        justify-content: center;
        margin: 0 auto;

        .input{        
          font-weight: 500;
          border-radius: 5px;
          background:$gray2;
          color:white;
          padding:0px 10px;
          
          &:hover{
            background: $gray3;
          }
          &:focus{
            -webkit-appearance: none;
            outline:none;
            background: $gray3;
          }
        }
          .label{
            font-weight: 600;
            font-size: large;
            width:20%;
            margin-left:10px;
            margin-top:5px;
            color:$gray5;
        }
      }

      .action{
        flex:1;
        margin: 0 auto;
        margin-bottom:20px;
        
        .buttonGroup{
          display:flex;
          flex-direction: row;
        }

        .button {
          margin:0 5px;
          background:$gray3;
          padding:0 15px;
          border-radius: 5px;
          font-weight: 500;

          &:hover{
            background:$gray5;
          }

        }
      }
    }
  }

  </style>
  