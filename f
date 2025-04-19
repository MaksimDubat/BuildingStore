[1mdiff --git a/BuildingStore/ProductService/Domain/Interfaces/IBaseRepository.cs b/BuildingStore/ProductService/Domain/Interfaces/IBaseRepository.cs[m
[1mindex 674296b..08dd7fc 100644[m
[1m--- a/BuildingStore/ProductService/Domain/Interfaces/IBaseRepository.cs[m
[1m+++ b/BuildingStore/ProductService/Domain/Interfaces/IBaseRepository.cs[m
[36m@@ -43,5 +43,10 @@[m [mnamespace ProductService.Domain.Interfaces[m
         /// <param name="predicate"></param>[m
         /// <param name="cancellation"></param>[m
         Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation);[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// Использование спецификаций.[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        /// <param name="specification"></param>[m
[32m+[m[32m        Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellation);[m
     }[m
 }[m
[1mdiff --git a/BuildingStore/ProductService/Infrastructure/Configurations/CategoryConfiguration.cs b/BuildingStore/ProductService/Infrastructure/Configurations/CategoryConfiguration.cs[m
[1mindex a5c883c..f8ecdc7 100644[m
[1m--- a/BuildingStore/ProductService/Infrastructure/Configurations/CategoryConfiguration.cs[m
[1m+++ b/BuildingStore/ProductService/Infrastructure/Configurations/CategoryConfiguration.cs[m
[36m@@ -19,7 +19,7 @@[m [mnamespace ProductService.Infrastructure.Configurations[m
             builder.HasMany(c => c.Products)[m
                 .WithOne(c => c.Category)[m
                 .HasForeignKey(c => c.CategoryId)[m
[31m-                .OnDelete(DeleteBehavior.Restrict);[m
[32m+[m[32m                .OnDelete(DeleteBehavior.Cascade);[m
         }[m
     }[m
 }[m
[1mdiff --git a/BuildingStore/ProductService/Infrastructure/Repositories/BaseRepository.cs b/BuildingStore/ProductService/Infrastructure/Repositories/BaseRepository.cs[m
[1mindex fdbf11c..a550230 100644[m
[1m--- a/BuildingStore/ProductService/Infrastructure/Repositories/BaseRepository.cs[m
[1m+++ b/BuildingStore/ProductService/Infrastructure/Repositories/BaseRepository.cs[m
[36m@@ -47,6 +47,12 @@[m [mnamespace ProductService.Infrastructure.Repositories[m
         {[m
             return await _context.Set<T>().FindAsync(id, cancellation);[m
         }[m
[32m+[m[32m        /// <inheritdoc/>[m
[32m+[m[32m        public async Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellation)[m
[32m+[m[32m        {[m
[32m+[m[32m            return await _context.Set<T>().Where(specification.Criteria).ToListAsync();[m
[32m+[m[32m        }[m
[32m+[m
         /// <inheritdoc/>[m
         public async Task UpdateAsync(T entity, CancellationToken cancellation)[m
         {[m
[1mdiff --git a/BuildingStore/ProductService/ProductService.csproj b/BuildingStore/ProductService/ProductService.csproj[m
[1mindex 59036bb..54c770e 100644[m
[1m--- a/BuildingStore/ProductService/ProductService.csproj[m
[1m+++ b/BuildingStore/ProductService/ProductService.csproj[m
[36m@@ -21,8 +21,4 @@[m
     <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />[m
   </ItemGroup>[m
 [m
[31m-  <ItemGroup>[m
[31m-    <Folder Include="Controllers\" />[m
[31m-  </ItemGroup>[m
[31m-[m
 </Project>[m
[1mdiff --git a/BuildingStore/ProductService/WebAPI/Controllers/CategoryController.cs b/BuildingStore/ProductService/WebAPI/Controllers/CategoryController.cs[m
[1mindex 4df2b2a..86864aa 100644[m
[1m--- a/BuildingStore/ProductService/WebAPI/Controllers/CategoryController.cs[m
[1m+++ b/BuildingStore/ProductService/WebAPI/Controllers/CategoryController.cs[m
[36m@@ -3,7 +3,6 @@[m [musing Microsoft.AspNetCore.Mvc;[m
 using ProductService.Application.DTOs;[m
 using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;[m
 using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Queries;[m
[31m-using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;[m
 [m
 namespace ProductService.WebAPI.Controllers[m
 {[m
