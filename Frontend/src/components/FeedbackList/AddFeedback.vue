<template>
  <div class="addPage">
    <div class="inputForm">
      <div class="description">
        <span class="label">Description</span>
        <input
          v-model="formFeedback.description"
          type="text"
          class="feedback-item"
        />
      </div>
      
      <div class="description">
        <span class="label">Duedate</span>
        <input
          v-model="formFeedback.dueDate"
          type="datetime-local"
          class="feedback-item"
        />      
      </div>
      <button class="button" @click.prevent="submitForm()">Add</button>
    </div>
    
    <div v-if="errors.length" class="error">
      <li v-for="error in errors" :key="error">{{ error }}</li>
    </div>
  </div>
</template>

<script lang="ts">
import useFeedbacks from '@/api/useFeedbacks';
import FeedbackInput from '@/interfaces/FeedbackInput';

import { useRouter } from 'vue-router';
import { defineComponent, ref, Ref } from 'vue';

export default defineComponent({
  setup() {
    const { addFeedback } = useFeedbacks();
    const router = useRouter();

    const formFeedback: Ref<FeedbackInput> = ref({});

    const errors = ref<String[]>([]);

    const submitForm = async () => {
      errors.value = [];

      if (formFeedback.value.description && formFeedback.value.dueDate) {
        await addFeedback(formFeedback.value);
        router.push('/');
        formFeedback.value = ref({});
      }

      if (formFeedback.value.description == undefined || formFeedback.value.description == '') {
        errors.value.push('Description required.');
      }
      if (formFeedback.value.dueDate == undefined) {
        errors.value.push('Duedate required.');
      }
    };
    return { formFeedback, submitForm, errors };
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
.addPage {
  display: flex;
  flex-direction: column;
  width: 100%;
  height: 100%;
  justify-content: flex-start;

  .inputForm {
    display: flex;
    flex-direction: column;
    max-height: 200px;
    padding: 10px;
    background-color: $gray1;

    border-radius: 5px;
    .description{
      display:flex;
      flex-direction: row;
      justify-items: flex-start;
      color:$gray5;

      .label{
        font-weight: 600;
        font-size: large;
        margin: 10px;
        width:20%;
        height: 70%;
        padding-left:40px;
        padding-right:50px;
      }

      .feedback-item {
        font-weight: 500;
        font-size: large;
        margin-top:5px;
        margin-left:auto;
        width: 70%;
        height: 70%;
        border-radius: 5px;
        background:$gray2;
        color:white;

        &:hover{
          background: $gray3;
        }
        &:focus{
          -webkit-appearance: none;
            outline:none;
          background: $gray3;
        }
      }
    }

    .button {
      width: 20%;
      margin: auto;
      background:$gray3;
      border-radius: 5px;
      margin-top:5px;
      &:hover{
        background:$gray5;
      }
    }
  }
}
input {
  padding-left: 10px;
  padding-right: 10px;
}
li {
  display: flex;
  flex-direction: row;
}
</style>
