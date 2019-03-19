<template>
  <div class="home">
    <div class="main-option">
      <h2>Host a captioning session</h2>
      <button @click="login">Host</button>
    </div>
    <div class="main-option">
      <h2>Join a captioning session</h2>
      <div>
        <button @click="join">Join</button>
      </div>
    </div>
  </div>
</template>

<script>

export default {
  name: 'home',
  components: {
  },
  data() {
    return {
      joinCode: ''
    }
  },
  methods: {
    login() {
      this.$router.push('host')
    },
    join() {
      this.$router.push('join')
    }
  }
}
</script>

<style>
div.main-option {
  border: 2px solid #ddd;
  border-radius: 10px;
  padding: 24px;
  max-width: 740px;
  margin: 24px auto;
}

.main-option h2 {
  margin: 0px 0px 12px 0px;
}

.main-option button {
  padding: 12px;
  border-radius: 8px;
  width: 300px;
  font-size: 16px;
}

.main-option input[type=text] {
  padding: 0px;
  margin: 12px;
  border-radius: 8px;
  height: 48px;
  width: 300px;
  font-size: 28px;
  text-align: center;
}
</style>
