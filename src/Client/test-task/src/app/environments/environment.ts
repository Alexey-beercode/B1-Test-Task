export const environment = {
  production: false, // true for environment.prod.ts
  apiUrls: {
    statistics: 'http://localhost:8083/api/Statistics',
    fileMerge: 'http://localhost:8083/api/FileMerge',
    fileImport: 'http://localhost:8083/api/FileImport',
    fileGenerator: 'http://localhost:8083/api/FileGenerator',
    bankStatementFiles: 'http://localhost:8082/api/BankStatementFiles',
    export: 'http://localhost:8082/api/Export',
    bankStatementEntries: 'http://localhost:8082/api/BankStatementEntries',
    base: 'http://localhost:8083',
  }
};
