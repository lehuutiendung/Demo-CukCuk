<template>
  <div>
      <!-- id="firstField" -->
    <input
      type="text"
      autocomplete=""
      title="Trường bắt buộc phải nhập!"
      class="col-1 autofocus"
      v-bind="$attrs"
    />
  </div>
</template>
<script>
export default {
    name: 'InputField',
};
</script>
<style>
</style>