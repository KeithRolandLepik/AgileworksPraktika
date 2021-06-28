import { reactive, ref, toRefs, PropType, Ref } from 'vue';
import useApi from './api';
import FeedbackData from '../interfaces/FeedbackData';
import FeedbackInput from '@/interfaces/FeedbackInput';
import FeedbackUpdate from '@/interfaces/FeedbackUpdate';
import UserModel from '@/interfaces/UserModel';
import UserRequest from '@/interfaces/UserRequest';

const state = reactive({
  user: ref<UserModel>(),
  errors: ref<UserModel[]>()
});

export default function useUsers() {
  const apiAuthUser = useApi<UserModel>('Users');

  const authenticateUser = async (userRequest: UserRequest) => {
    const requestOptions = {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(userRequest),
    };
    const apiAuthUser = useApi<UserModel>(
      'Users/authenticate',
      requestOptions,
    );
    await apiAuthUser.request();
    if (apiAuthUser.response.value!) {
      var response = apiAuthUser.response.value!;
      state.user = response;
      localStorage.setItem("user", JSON.stringify(state.user))
    }
  };

  const registerUser = async (userRequest: UserRequest) => {
    const requestOptions = {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(userRequest),
    };
    const apiAuthUser = useApi<UserModel[]>(
      'Users/register',
      requestOptions,
    );
    await apiAuthUser.request();
    if(apiAuthUser.code.value === 200){
          var response = apiAuthUser.response.value!;
          state.user = response[0];
    }
    else{
      var response = apiAuthUser.response.value!;
      response.forEach(e => {
        state.errors?.push(e.errorMessage)
      });
    }
  };
  const logoutUser = () => {
      state.user = undefined;
      localStorage.removeItem("user");
  }
  return {
      registerUser,logoutUser,
      authenticateUser,
      ...toRefs(state),
  };
}
