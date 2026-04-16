export function SkeletonTable({ rows = 5, cols = 4 }: { rows?: number; cols?: number }) {
  return (
    <div className="rounded-lg border bg-card shadow-sm overflow-hidden">
      <div className="border-b bg-muted/50 p-4 flex gap-4">
        {Array.from({ length: cols }).map((_, i) => (
          <div key={i} className="h-4 flex-1 rounded bg-muted animate-pulse" />
        ))}
      </div>
      {Array.from({ length: rows }).map((_, r) => (
        <div key={r} className="border-b p-4 flex gap-4">
          {Array.from({ length: cols }).map((_, c) => (
            <div key={c} className="h-4 flex-1 rounded bg-muted animate-pulse" />
          ))}
        </div>
      ))}
    </div>
  );
}
