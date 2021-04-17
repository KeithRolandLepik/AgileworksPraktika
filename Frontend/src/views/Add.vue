<template>
  <div class="page">
    <div class="header">
      <span class="headerText">Add a Feedback</span>
    </div>
    <div class="content">
      <AddFeedback />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import AddFeedback from '../components/FeedbackList/AddFeedback.vue';
export default defineComponent({
  components: {
    AddFeedback,
  },
  setup() {
    return {};
  },
});
</script>

<style scoped lang="scss">$gray: rgba(28, 28, 30);
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
  background:$gray;
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
    height: 80%;
    width: 60%;
    display: flex;
    justify-content: center;
    margin: auto;
  }
}
</style>
