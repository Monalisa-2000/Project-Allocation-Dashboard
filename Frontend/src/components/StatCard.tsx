import type { ReactNode } from 'react';
import type { LucideIcon } from 'lucide-react';

interface StatCardProps {
  label: string;
  value: number | string;
  icon: LucideIcon;
  color?: 'primary' | 'success';
}

export function StatCard({ label, value, icon: Icon, color = 'primary' }: StatCardProps) {
  const colorClasses = color === 'success'
    ? 'text-success bg-success/10'
    : 'text-primary bg-primary/10';

  return (
    <div className="rounded-lg border bg-card p-6 shadow-sm relative overflow-hidden">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-sm font-medium text-muted-foreground">{label}</p>
          <p className="mt-1 text-3xl font-bold text-card-foreground">{value}</p>
        </div>
        <div className={`flex h-12 w-12 items-center justify-center rounded-lg ${colorClasses}`}>
          <Icon className="h-6 w-6" />
        </div>
      </div>
    </div>
  );
}
