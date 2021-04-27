import { ref, toRefs } from 'vue';


export default function getSetDateDifference() {

  const tNumber = ref(0)
  const tUnit = ref("")

  const calculateDateDifference = async (startDate: Date, endDate: Date) => {
      const end = ref(0)
      const start = ref(0)

      end.value = new Date(endDate).getTime();
      start.value = new Date(startDate).getTime();

     const timeAsSeconds = (end.value - start.value) / 1000
      var timeAbsValue = Math.abs(timeAsSeconds)

      switch (true) {
        case (timeAbsValue >= 172800): // 2 days +
          tUnit.value = "Days"
          tNumber.value = Math.floor(timeAsSeconds / (3600 * 24))
          return
        case (timeAbsValue < 172800 && timeAbsValue >= 86400): // 1 Day + Less than 2 days
          tUnit.value = "Day"
          tNumber.value =Math.floor(timeAsSeconds / (3600 * 24))
          return
        case (timeAbsValue < 86400 && timeAbsValue >= 7200): // Less than day
          tUnit.value = "Hours"
          tNumber.value = Math.floor(timeAsSeconds / (3600))
          return
        case (timeAbsValue < 7200 && timeAbsValue >= 3600): // Less than 2 hours
          tUnit.value = "Hour"
          tNumber.value = Math.floor(timeAsSeconds / (3600))
          return
        case (timeAbsValue < 3600 && timeAbsValue >= 120): // Less then hour
          tUnit.value = "Minutes"
          tNumber.value = Math.floor(timeAsSeconds / 60)
          return
        case (timeAbsValue < 120 && timeAbsValue >= 60): // Less than 2 minutes
          tUnit.value = "Minute"
          tNumber.value = Math.floor(timeAsSeconds / 60)     
          return
        case (timeAbsValue < 60 && timeAbsValue >= 2): // Less than minute
          tUnit.value = "Seconds"
          tNumber.value = Math.floor(timeAsSeconds)
          return
        case (timeAbsValue < 2): // 1 Second
          tUnit.value = "Second"
          tNumber.value = Math.floor(timeAsSeconds)
          return
        default:
          tUnit.value = "-"
          tNumber.value = Math.floor(timeAsSeconds)      
        }
    };

    const getTimeUnit = () => {
      return tUnit.value
    };
    
    const getTimeAsNumber = () => {
      return tNumber.value
    };

    return { calculateDateDifference, getTimeUnit, getTimeAsNumber };
}
