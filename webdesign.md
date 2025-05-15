3.1 Visual Identity (updated)

Colors  
Primary: Purple (#6366f1), Pink (#ec4899), White (#ffffff)  
Secondary: Gray (#4b5563), Black (#0f172a), Green (#10b981)

CSS Variables  
```css
:root {
  --purple: #6366f1;
  --pink: #ec4899;
  --bg: #121212;
  --surface: #1f1f1f;
  --text: #e0e0e0;
  --muted: #b3b3b3;
  --accent: #10b981;
}
```

Gradient Helper  
```css
.gradient-text {
  background: linear-gradient(90deg, var(--purple), var(--pink));
  -webkit-background-clip: text;
  color: transparent;
}
```

Typography  
Font Family: Inter (Google Fonts)  
Weights: 400 (Regular), 500 (Medium), 700 (Bold)  
Sizes:  
- Headings: text-4xl (2.25rem), text-3xl (1.875rem)  
- Body: text-lg (1.125rem), text-sm (0.875rem)

Imagery & Assets  
- Hero placeholder: `/wwwroot/img/hero_placeholder.jpg`  
- Lucide icons: CDN import and `<i class="lucide-ICON"></i>`  
- Ecosystem diagram placeholder: `<canvas id="ecosystemGraph"></canvas>` for About page  
- Logo: TBD

Style Notes  
- Dark, tech-forward aesthetic with neon accents  
- Placeholder ready for Framer Motion animations  
- Ensure WCAG 2.1 contrast compliance (≥4.5:1 ratio)

3.2 Tone and Voice

Tone: Professional, approachable, empowering, and innovative.  
Voice: Confident and artist-centric, emphasizing transparency and accessibility.  

Examples:  
- “Musika levels the playing field” (Home page)  
- “Built for working artists—the ones juggling metadata, missing royalties” (About page)

Key Phrases: “Find Your Sound,” “A More Equitable Music Industry,” “Empowering Artists.”
