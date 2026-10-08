# Workspace client structure

- `app/`: application root component and application-wide composition.
- `assets/icons/`: imported vector assets.
- `assets/images/`: imported raster assets.
- `components/layout/`: reusable application layout components.
- `components/ui/`: presentational and reusable UI components.
- `features/`: business capabilities organized as vertical slices.
- `hooks/`: reusable React hooks.
- `lib/`: framework-agnostic client utilities.
- `pages/`: route-level page components.
- `routes/`: React Router route definitions and guards.
- `services/`: HTTP clients and external client-side integrations.
- `styles/`: global styles and Tailwind imports.
- `types/`: shared TypeScript types.

Keep feature-specific components, hooks, services, and types within their feature until they are reused across features.
