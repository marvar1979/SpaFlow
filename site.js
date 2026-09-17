document.addEventListener("DOMContentLoaded", () => {
  const inputs = document.querySelectorAll('input[type="datetime-local"]');
  inputs.forEach(i => {
    if (!i.value) {
      const d = new Date(Date.now() + 60 * 60 * 1000);
      d.setMinutes(Math.ceil(d.getMinutes() / 15) * 15, 0, 0);
      i.value = new Date(d.getTime() - d.getTimezoneOffset() * 60000).toISOString().slice(0,16);
    }
  });
});
