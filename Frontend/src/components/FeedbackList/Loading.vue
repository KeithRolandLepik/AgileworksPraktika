<template>       
        <span class="feedback-item">...Loading component</span>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

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