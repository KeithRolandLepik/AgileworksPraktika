import { ref, Ref } from 'vue';
export type ApiRequest = () => Promise<void>;

export interface UsableAPI<T> {
  response: Ref<T | undefined>;
  request: ApiRequest;
  code: Ref<number>;
}

let apiUrl = 'https://localhost:44347/api/';

export function setApiUrl(url: string) {
  apiUrl = url;
}

export default function useApi<T>(
  url: string,
  options?: RequestInit,
): UsableAPI<T> {
  const response: Ref<T | undefined> = ref();
  const code: Ref<any> = ref();

  const request: ApiRequest = async () => {
  
    const res =
    await fetch(apiUrl + url, options);
     const data = await res.json().catch((error) => {
      console.log('Api call error ', error);
    });

    code.value = res.status
    response.value = data;
  };

  return { code ,response, request };
}
