<template>
  <div id="app">
    <div id="nav">
      <div>
        <h1><router-link to="/">{{ title }}</router-link></h1>
      </div>
    </div>
    <div id="main">
      <router-view :client-id="clientId" />
    </div>
  </div>
</template>

<script>
import uuidv4 from 'uuid/v4'
import constants from './lib/constants'

export default {
  data() {
    return {
      clientId: uuidv4(),
      title: constants.title
    }
  }
}
</script>

<style>
body {
  margin: 0px;
  padding: 0px;
}

#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  width: 100%;
}
#nav {
  position: fixed;
  top: 0;
  width: 100%;
  min-height: 100px;
  z-index: 1000;
  background-color: #fff;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav h1 a {
  text-decoration: none;
  padding-top: 12px;
}

#main {
  margin-top: 100px;
}

button {
  padding: 12px;
  border-radius: 8px;
  width: 200px;
  font-size: 16px;
}

input[type=text], input[type=password], select {
  padding: 0px;
  margin: 12px;
  border-radius: 8px;
  height: 48px;
  width: 200px;
  font-size: 18px;
  text-align: center;
}

.caption {
  margin: 48px auto;
  max-width: 1024px;
  font-size: 48px;
  text-align: left;
  padding: 24px;
}
</style>
