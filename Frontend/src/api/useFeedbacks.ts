import { reactive, ref, toRefs, PropType, Ref } from 'vue';
import useApi from './api';
import FeedbackData from '../interfaces/FeedbackData';
import FeedbackInput from '@/interfaces/FeedbackInput';
import FeedbackUpdate from '@/interfaces/FeedbackUpdate';

const state = reactive({
  feedbacks: Array<FeedbackData>(),
});

export default function useFeedbacks() {
  const apiGetFeedbacks = useApi<FeedbackData[]>('Feedback');
  const loadFeedbacks = async () => {
    await apiGetFeedbacks.request();
    if (apiGetFeedbacks.response.value) {
      state.feedbacks = apiGetFeedbacks.response.value!;
    }
  };

  const addFeedback = async (feedback: FeedbackInput) => {
    const requestOptions = {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(feedback),
    };
    const apiGetFeedbacks = useApi<FeedbackData>(
      'Feedback',
      requestOptions,
    );
    await apiGetFeedbacks.request();
    if (apiGetFeedbacks.response.value!) {
      var response = apiGetFeedbacks.response.value!;
      state.feedbacks.push(response);
    }
  };

  const updateFeedback = async (id: Number, feedback: FeedbackUpdate) => {
    const requestOptions = {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(feedback),
    };

    const apiGetFeedbacks = useApi('Feedback/' + id, requestOptions);
    await apiGetFeedbacks.request();
  };
  return {
    loadFeedbacks,
    addFeedback,
    updateFeedback,
    ...toRefs(state),
  };
}
