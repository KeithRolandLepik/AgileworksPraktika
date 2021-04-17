import { reactive, ref, toRefs, PropType, Ref } from 'vue';
import useApi from './api';
import FeedbackData from '../interfaces/FeedbackData';

const state = reactive({
  feedbacks: Array<FeedbackData>(),
});

export default function useFeedbacks() {
  const apiGetFeedbacks = useApi<FeedbackData[]>('FeedbackDatas');
  const loadFeedbacks = async () => {
    await apiGetFeedbacks.request();
    if (apiGetFeedbacks.response.value) {
      state.feedbacks = apiGetFeedbacks.response.value!;
    }
  };

  const addFeedback = async (feedback: FeedbackData) => {
    const requestOptions = {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(feedback),
    };
    const apiGetFeedbacks = useApi<FeedbackData>(
      'FeedbackDatas',
      requestOptions,
    );
    await apiGetFeedbacks.request();
    if (apiGetFeedbacks.response.value!) {
      var response = apiGetFeedbacks.response.value!;
      state.feedbacks.push(response);
    }
  };

  const updateFeedback = async (id: Number, feedback: FeedbackData) => {
    const requestOptions = {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(feedback),
    };

    const apiGetFeedbacks = useApi('FeedbackDatas/' + id, requestOptions);
    await apiGetFeedbacks.request();
  };
  return {
    loadFeedbacks,
    addFeedback,
    updateFeedback,
    ...toRefs(state),
  };
}
