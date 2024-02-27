/** @type {import('tailwindcss').Config} */
export default {
  purge: ["./index.html", "./src/**/*.{vue,js,ts,jsx,tsx}"],
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
      container: {
        padding: "2.5rem",
        screens: {
          "3xl": "1920px",
        },
      },
      fontSize: {
        xxs: "10.8px",
        xs: "12px",
        sm: "14px",
        md: ["16px", "normal"],
        lg: ["20px", "normal"],
      },
      colors: {
        primary: {
          100: "#F0F5FF",
          200: "#E5EEFF",
          DEFAULT: "#204ECF",
          800: "#1D5BD6",
          950: "#0400D0",
          1000: "#0300AC",
        },
        red: {
          DEFAULT: "#FF0000",
        },
        purple: {
          500: "#524FFF",
        },
        gray: {
          200: "#EAECF0",
          300: "#606060",
          500: "#667085",
          700: "#344054",
        },
        orange: {
          100: "#FFF9E9",
          500: "#D27100",
        },
        warning: {
          50: "#FFFAEB",
          100: "#FEF0C7",
          800: "#93370D",
        },
        success: {
          50: "#ECFDF3",
          100: "#D1FADF",
          600: "#039855",
        },
        neutral: {
          700: "#3F444D",
          500: "#B7B7B7",
          1000: "#0A0D14",
        },
      },
    },
  },
  variants: {
    extend: {},
  },
}
