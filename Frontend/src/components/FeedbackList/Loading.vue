<template>       
        <span class="feedback-item">...Loading component</span>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue'
import FeedbackData from '../../interfaces/FeedbackData'
import getDateDifference from '../getDateDifference';

export default defineComponent({
   setup () {
        
        return {}
    }
})
</script>

<style scoped lang="scss">
.feedback-item{
    font-weight:500;
    font-size: small;
    margin:auto;            
    margin:10px;            
    min-height:70px;
    width:100%;
    padding:10px;
    }
</style>