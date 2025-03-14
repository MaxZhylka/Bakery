/** @type {import('tailwindcss').Config} */
module.exports = {
  corePlugins: {
    preflight: false // disable preflight to avoid conflicts with main scss styles
  },
  content: ['./src/**/*.{html,ts,scss}'],
  theme: {
    extend: {
      scrollSnapType: {
        y: 'y mandatory',
      },
      scrollSnapAlign: {
        end: 'end',
      },
    },
  },
  plugins: [],
};