export function SkeletonCard() {
  return (
    <div className="rounded-lg border bg-card p-6 shadow-sm animate-pulse">
      <div className="h-5 w-2/3 rounded bg-muted mb-3" />
      <div className="h-4 w-full rounded bg-muted mb-2" />
      <div className="h-4 w-1/2 rounded bg-muted mb-4" />
      <div className="h-3 w-1/3 rounded bg-muted mb-3" />
      <div className="h-9 w-full rounded bg-muted" />
    </div>
  );
}
