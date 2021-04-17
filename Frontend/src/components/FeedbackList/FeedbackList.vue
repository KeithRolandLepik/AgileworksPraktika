<template>
  <div class="list-container">
    <ul class="feedback-list">
      <li class="feedback-item add" @click.prevent="pushToAdd()">
        <h1>Add a new feedback</h1>
      </li>
      <Suspense>
        <template #fallback>
          <div><span>Loading...</span></div>
        </template>
        <template #default>
          <AsyncFeedback
            v-for="item in feedbacks"
            :key="item.id"
            :feedbackData="item"
          />
        </template>
      </Suspense>
    </ul>
  </div>
</template>

<script lang="ts">
import { defineAsyncComponent, defineComponent } from 'vue';
import { useRouter } from 'vue-router';
import useFeedbacks from '../../api/useFeedbacks';
import Loading from './Loading.vue';

const AsyncFeedback = defineAsyncComponent({
  loader: () =>
    import('./Feedback.vue' /* webpackChunkName: "asyncFeedback" */),
  loadingComponent: Loading,
  delay: 200,
  suspensible: false,
});

export default defineComponent({
  components: {
    AsyncFeedback,
  },

  async setup() {
    const router = useRouter();

    const { loadFeedbacks, feedbacks } = await useFeedbacks();
    await loadFeedbacks();

    console.log(feedbacks);

    const pushToAdd = () => {
      router.push('/add');
    };

    return { feedbacks, pushToAdd };
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
.list-container {
  height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: space-between;

  .feedback-list {
    display: flex;
    height: 100%;
    width: 100%;
    flex-direction: column;
    margin: 0 auto;

    .feedback-item {
      min-height: 70px;
      max-height: 140px;
      width: 100%;
      display: flex;
      flex-direction: row;
      justify-content: space-around;
      background-color:$gray1;
      border-radius: 5px;
      margin:5px;

      &:hover {
        background: $gray2;
        cursor: pointer;
        &.overDue {
          background-color: #f67d70af;
        }
      }
      &.overDue {
        background-color: #f67e70;
      }

      &.add {
        text-align: center;
        h1 {
          margin: auto;
          font-weight: 500;
          font-size: x-large;
          font: bold;
        }
        &:hover {
            opacity: 0.5;
          }
      }
    }
  }
}
</style>
