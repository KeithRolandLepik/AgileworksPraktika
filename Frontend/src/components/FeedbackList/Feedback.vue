<template>
  <li
    v-if="!feedbackData.completed"
    class="feedback-item"
    @click="changeToCompleted(feedbackData.id)"
  >    
    <div class="feedback-item-date" :class="feedbackData.overdue ? 'overDue' : ''">
      <div class="data">{{ getTimeAsNumber() }} {{ getTimeUnit() }}</div></div>
      
    <div class="feedback-item-description"><div class="data">{{feedbackData.description}}</div></div>

    <span class="complete">Complete</span>
  </li>
</template>
<script lang="ts">
import useFeedbacks from '@/api/useFeedbacks';
import { defineComponent, PropType, ref } from 'vue';
import FeedbackData from '../../interfaces/FeedbackData';
import getSetDateDifference from '../getSetDateDifference';

export default defineComponent({
  components: {},
  props: {
    feedbackData: {
      type: Object as () => PropType<FeedbackData>,
      required: true,
    },
  },
  async setup(props) {
    const { updateFeedback, feedbacks } = useFeedbacks();

    const {
      calculateDateDifference,
      getTimeUnit,
      getTimeAsNumber,
    } = getSetDateDifference();

    const changeToCompleted = async (id: Number) => {
      if(window.confirm("Do you really want to change this Feedback to Completed?")){
        var feedback = feedbacks.value.find((x) => x.id == id);
        feedback.completed = true;

        if (feedback) {
          await updateFeedback(id, feedback);
        }
      }
    };

    await calculateDateDifference(new Date(), props.feedbackData.dueDate);
    const isTabOpen = ref(false);

    const openTab = () => {
      isTabOpen.value = !isTabOpen.value;
    };
    return {
      getTimeUnit,
      getTimeAsNumber,
      isTabOpen,
      openTab,
      changeToCompleted,
    };
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
.feedback-item {
      overflow:hidden;
    min-height: 90px;
    max-height: 240px;
    width: 70%;
    display: flex;
    flex-wrap: wrap;
    flex-direction: row;
    justify-content: space-around;
    background-color: $gray1;

  .feedback-item-description {
    font-weight: 500;
    font-size: x-large;
    margin: auto;
    flex:2;
    .data{
      width:50%
    }
  }

  .feedback-item-date {
    flex:1;
    height:100%;
    width:100%;
    padding:auto;
    display:flex;
    flex-direction: column;
    justify-items: center;
    min-width:100px;
    max-width:100px;
    margin-right:50px;
    background:$gray2;
    
      &.overDue{
        color: #ffffff;
        background-color: #e54b4b;
    }
    .data {
      font-weight: 500;
      font-size: large;
      padding-left: 10px;
      margin-top:auto;
      margin-bottom: auto;
    }
  }


  .complete {
      font-weight: 400;
      font-size: x-large;
      margin-top: auto;
      margin-bottom: auto;
      visibility: hidden;
      width:0;
  }

  &:hover {
    cursor: pointer;
    .feedback-item-description{
      width:10%;
      opacity:0.2;
    }
    .feedback-item-date{
      opacity:0.2;
    }
    &.overDue {
      background-color: #f67d70;
    }

    .complete {
      visibility: visible;    
      width:40%;      
      margin-top: auto;
      margin-bottom: auto;
      position: relative;
    }
  }

  &.overDue {
    background-color: #fa8b7f;
  }
}
</style>
