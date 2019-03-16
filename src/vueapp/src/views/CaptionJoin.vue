<template>
  <div class="caption-join">
    <h1>Join a session</h1>
    <h2>{{ code }}</h2>
  </div>
</template>

<script>
export default {
  data() {
    return {
      code: ''
    }
  },
  mounted() {
    this.code = this.$route.params.code
  }
}
</script>
